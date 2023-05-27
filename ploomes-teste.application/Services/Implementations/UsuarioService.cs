using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ploomes_teste.application.DTOs.Usuario;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Helpers;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;

namespace ploomes_teste.application.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRegistrarAvaliadorNegocio _registrarAvaliadorNegocio;
        private readonly IMapper _mapper;

        public UsuarioService(IConfiguration config,
                                UserManager<Usuario> userManager,
                                SignInManager<Usuario> signInManager,
                                IMapper mapper,
                                RoleManager<IdentityRole> roleManager,
                                IRegistrarAvaliadorNegocio registrarAvaliadorNegocio)
        {   
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _roleManager = roleManager;
            _registrarAvaliadorNegocio = registrarAvaliadorNegocio;
        }


        public async Task<UsuarioDTO> GetUser(ClaimsPrincipal principal)
        {
           

            var user = await _userManager.GetUserAsync(principal);

            if (user == null) throw new UnauthorizedException("O usuário não está logado");

            return _mapper.Map<UsuarioDTO>(user);
            
        }

        [AllowAnonymous]
        [Route("registrar/proprietario")]
        [HttpPost]
        public async Task<UsuarioDTO> RegistrarProprietario(RegistrarProprietarioDTO userDTO)
        {
            
            var user = new Usuario
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                NomeCompleto = userDTO.NomeCompleto,
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

                return userToReturn;
            }
            throw new BusinessException<RegistrarProprietarioDTO>(result.Errors.Select(u => TranslateErrorMessage(u.Code)).ToArray(),userDTO);
        
        }

        [AllowAnonymous]
        [Route("registrar/avaliador")]
        [HttpPost]
        public async Task<UsuarioDTO> RegistrarAvaliador(RegistrarAvaliadorDTO userDTO)
        {
            
            var user = new Usuario
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                NomeCompleto = userDTO.NomeCompleto,
                LatitudeMoradia = userDTO.Latitude,
                LongitudeMoradia = userDTO.Longitude
            };
            List<string> erros = _registrarAvaliadorNegocio.Validate(user);
            var result = await _userManager.CreateAsync(user, userDTO.Password);

            var userToReturn = _mapper.Map<UsuarioDTO>(user);

            if (result.Succeeded && erros.Count == 0)
            {
                //-------------------atribuir role ao user------------------------------
                var applicationRole = await _roleManager.FindByNameAsync("Avaliador");
                if (applicationRole != null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                }
                
                return userToReturn;
            }

            foreach(IdentityError identityerrors in result.Errors)
                erros.Add(TranslateErrorMessage(identityerrors.Code));

            throw new BusinessException<RegistrarAvaliadorDTO>(erros.ToArray(),userDTO);
           
        }

      
        public async Task<UsuarioLogadoDTO> Login(UsuarioLoginDTO userLoginDTO)
        {
           
            var user = await _userManager.FindByNameAsync(userLoginDTO.UserName);
            if( user == null)
                throw new UnauthorizedException("O nome de usuário não existe ou a senha está incorreta");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDTO.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userLoginDTO.UserName.ToUpper());

                var userToReturn = _mapper.Map<UsuarioDTO>(appUser);

                return new UsuarioLogadoDTO(){Usuario = userToReturn,Token = GenerateJWToken(appUser).Result};
            }
            throw new UnauthorizedException("O nome de usuário não existe ou a senha está incorreta");
           
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
        private string TranslateErrorMessage(string codeError)
        {
            string message = string.Empty;
            switch (codeError)
            {
                case "DefaultError":
                    message =  "Um erro desconhecido ocorreu.";
                    break;
                case "ConcurrencyFailure":
                    message = "Falha de concorrência otimista, o objeto foi modificado.";
                    break;
                case "InvalidToken":
                    message = "Token inválido.";
                    break;
                case "LoginAlreadyAssociated":
                    message = "Já existe um usuário com este login.";
                    break;
                case "InvalidUserName":
                    message = $"Este login é inválido, um login deve conter apenas letras ou dígitos.";
                    break;
                case "InvalidEmail":
                    message = "E-mail inválido.";
                    break;
                case "DuplicateUserName":
                    message = "Este login já está sendo utilizado.";
                    break;
                case "DuplicateEmail":
                    message = $"Este email já está sendo utilizado.";
                    break;
                case "InvalidRoleName":
                    message = "Esta permissão é inválida.";
                    break;
                case "DuplicateRoleName":
                    message = "Esta permissão já está sendo Utilizada";
                    break;                
                case "UserAlreadyInRole":
                    message = "Usuário já possui esta permissão.";
                    break;
                case "UserNotInRole":
                    message = "Usuário não tem esta permissão.";
                    break;
                case "UserLockoutNotEnabled":
                    message = "Lockout não está habilitado para este usuário.";
                    break;
                case "UserAlreadyHasPassword":
                    message = "Usuário já possui uma senha definida.";
                    break;
                case "PasswordMismatch":
                    message = "Senha incorreta.";
                    break;
                case "PasswordTooShort":
                    message = "Senha muito curta.";
                    break;
                case "PasswordRequiresNonAlphanumeric":
                    message = "Senhas devem conter ao menos um caracter não alfanumérico.";
                    break;
                case "PasswordRequiresDigit":
                    message = "Senhas devem conter ao menos um digito ('0'-'9').";
                    break;
                case "PasswordRequiresLower":
                    message = "Senhas devem conter ao menos um caracter em caixa baixa ('a'-'z').";
                    break;
                case "PasswordRequiresUpper":
                    message = "Senhas devem conter ao menos um caracter em caixa alta ('A'-'Z').";
                    break;
                default:
                    message = "Um erro desconhecido ocorreu.";
                    break;

            }
            return message;
        }
    }
}