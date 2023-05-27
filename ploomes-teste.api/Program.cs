using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ploomes_teste.persistence.Contexts;
using ploomes_teste.domain;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ploomes_teste.persistence.Contracts;
using ploomes_teste.persistence.Implementations;
using ploomes_teste.application.Services.Implementations;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.negocio.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inje��o de dependencia

// Reposit�rios - Classes de Acesso ao banco
builder.Services.AddScoped<IAvaliacaoRepository, AvaliacaoRepository>();
builder.Services.AddScoped<ILugarRepository, LugarRepository>();

// Servicos
builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
builder.Services.AddScoped<ILugarService, LugarService>();


// Camada de Negocio
builder.Services.AddScoped<ICriarAvaliacaoNegocio, CriarAvaliacaoNegocio>();
builder.Services.AddScoped<IAlterarAvaliacaoNegocio, AlterarAvaliacaoNegocio>();
builder.Services.AddScoped<ICriarLugarNegocio, CriarLugarNegocio>();
builder.Services.AddScoped<IAlterarLugarNegocio, AlterarLugarNegocio>();

builder.Services.AddScoped<ILugarService, LugarService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors();

builder.Services.AddControllers(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    })
    .ConfigureApiBehaviorOptions(opt => {
            opt.InvalidModelStateResponseFactory = (context =>
                                                        new BadRequestObjectResult(context.ModelState.Values.SelectMany(x => x.Errors)
                                                                                            .Select(x => x.ErrorMessage)));
    });
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<PloomesContext>()
    .AddDefaultTokenProviders();
    
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value);

builder.Services.AddDbContext<PloomesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PloomesDB"))
);
IdentityBuilder builderIdentity = builder.Services.AddIdentityCore<Usuario>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 3;
});
builderIdentity = new IdentityBuilder(builderIdentity.UserType, typeof(IdentityRole), builderIdentity.Services);
builderIdentity.AddEntityFrameworkStores<PloomesContext>();
builderIdentity.AddRoleValidator<RoleValidator<IdentityRole>>();
builderIdentity.AddRoleManager<RoleManager<IdentityRole>>();
builderIdentity.AddSignInManager<SignInManager<Usuario>>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/usuario/login");
    options.AccessDeniedPath = new PathString("/usuario/login");

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
		
var app = builder.Build();

var roleManager = app.Services.GetRequiredService<RoleManager<IdentityRole>>();
var userManager = app.Services.GetRequiredService<UserManager<Usuario>>();
string[] rolesNames = { "Proprietario", "Avaliador" };
IdentityResult result;
foreach (var namesRole in rolesNames)
{
    var roleExist = await roleManager.RoleExistsAsync(namesRole);
    if (!roleExist)
    {
        result = await roleManager.CreateAsync(new IdentityRole(namesRole));
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});            
if (app.Environment.IsDevelopment())
{
    app.UseCors(opt => {
            opt.AllowAnyHeader();
            opt.AllowAnyOrigin();
            opt.AllowAnyMethod();
        }
    );
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ploomes v1"));
}else{
    app.UseCors();
    app.UseHsts();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
