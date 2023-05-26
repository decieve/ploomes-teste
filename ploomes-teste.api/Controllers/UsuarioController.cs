using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ploomes_teste.application.DTOs;
using ploomes_teste.domain;

namespace ploomes_teste.api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
     private readonly IConfiguration _config;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IMapper _mapper;

    public UsuarioController(IConfiguration config,
                            UserManager<Usuario> userManager,
                            SignInManager<Usuario> signInManager,
                            IMapper mapper,
                            RoleManager<IdentityRole> roleManager)
    {   
        _signInManager = signInManager;
        _userManager = userManager;
        _config = config;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    [Route("getuser")]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

            var user = await _userManager.GetUserAsync(principal);

            if (user == null) return Unauthorized();

            return Ok(_mapper.Map<UsuarioDTO>(user));
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [AllowAnonymous]
    [Route("registrar/proprietario")]
    [HttpPost]
    public async Task<IActionResult> RegistrarProprietario(RegistrarUsuarioDTO userDTO)
    {
        try
        {
            var user = new Usuario
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                NomeCompleto = userDTO.NomeCompleto
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            var userToReturn = _mapper.Map<UsuarioDTO>(user);
            if (result.Succeeded)
            {
                //-------------------atribuir role ao user------------------------------
                var applicationRole = await _roleManager.FindByNameAsync("Proprietario");
                if (applicationRole != null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                }

                return Created("get", userToReturn);
            }
            return BadRequest(result.Errors);
        }
  
        catch (Exception e)
        {
            return Problem("Algo deu errado");
        }
    }

    [AllowAnonymous]
    [Route("registrar/avaliador")]
    [HttpPost]
    public async Task<IActionResult> RegistrarAvaliador(RegistrarUsuarioDTO userDTO)
    {
        try
        {
            var user = new Usuario
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                NomeCompleto = userDTO.NomeCompleto
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            var userToReturn = _mapper.Map<UsuarioDTO>(user);
            if (result.Succeeded)
            {
                //-------------------atribuir role ao user------------------------------
                var applicationRole = await _roleManager.FindByNameAsync("Avaliador");
                if (applicationRole != null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                }
                
                return Created("get", userToReturn);
            }
            return BadRequest(result.Errors.Select(e => e.Code).ToArray());
        }
  
        catch (Exception e)
        {
            return Problem("Algo deu errado");
        }
    }

    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(UsuarioLoginDTO userLoginDTO)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userLoginDTO.UserName);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userLoginDTO.UserName.ToUpper());

                var userToReturn = _mapper.Map<UsuarioDTO>(appUser);
                return Ok(new
                {
                    token = GenerateJWToken(appUser).Result,
                    user = userToReturn
                });
            }
            return Unauthorized("O nome de usuário não existe ou a senha está incorreta");
        }
        catch (Exception e)
        {
            return Problem("Algo deu errado", e.Message);
        }
    }

    private async Task<string> GenerateJWToken(Usuario user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName)
        };
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
