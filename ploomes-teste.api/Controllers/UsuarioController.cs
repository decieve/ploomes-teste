using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ploomes_teste.application.DTOs.Usuario;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Services.Contracts;

namespace ploomes_teste.api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
   
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {   
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Obtém informações do usuário atualmente autenticado.
    /// </summary>
    /// <returns>As informações do usuário autenticado.</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     GET /get
    ///
    /// </remarks>
    /// <response code="200">Retorna as informações do usuário autenticado.</response>
    /// <response code="401">Se o usuário não estiver autenticado (possuir um token no header) ou não tiver permissão para acessar o recurso.</response>
    /// <response code="500">Se ocorrer um erro inesperado.</response>
    [Route("get")]
    [HttpGet]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;

            var user = await _usuarioService.GetUser(principal);
   
            return Ok(user);
        }
        catch(UnauthorizedException e){
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    /// <summary>
    /// Registra um novo proprietário.
    /// </summary>
    /// <param name="userDTO">Os dados do proprietário a ser registrado.</param>
    /// <returns>As informações do registro do proprietário.</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /registrar/proprietario
    ///     {
    ///         "userName":"petronio",
	///         "password":"1234",
	///         "nomeCompleto":"Petronio martins",
	///         "email":"emai"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Retorna as informações do registro do proprietário.</response>
    /// <response code="400">Se houver um erro de validação durante o registro.</response>
    /// <response code="500">Se ocorrer um erro inesperado.</response>
    [AllowAnonymous]
    [Route("registrar/proprietario")]
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarProprietario(RegistrarProprietarioDTO userDTO)
    {
        try
        {
            var registro = await _usuarioService.RegistrarProprietario(userDTO);
            
            return Ok(registro);
        }
        catch(BusinessException<RegistrarProprietarioDTO> b){
            return BadRequest(b.messages);
        }
        catch (Exception e)
        {
            return Problem("Algo deu errado");
        }
    }
    /// <summary>
    /// Registra um novo avaliador.
    /// </summary>
    /// <param name="userDTO">Os dados do avaliador a ser registrado.</param>
    /// <returns>As informações do registro do avaliador.</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /registrar/avaliador
    ///     {
    ///         "userName":"petronio",
	///         "password":"1234",
	///         "nomeCompleto":"Petronio martins",
	///         "email":"emai",
    ///         "latitude":31.0,
	///         "longitude":57.0
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Retorna as informações do registro do avaliador.</response>
    /// <response code="400">Se houver um erro de validação durante o registro.</response>
    /// <response code="500">Se ocorrer um erro inesperado.</response>
    [AllowAnonymous]
    [Route("registrar/avaliador")]
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RegistrarAvaliador(RegistrarAvaliadorDTO userDTO)
    {
        try
        {
            var registro = await _usuarioService.RegistrarAvaliador(userDTO);
            
            return Ok(registro);
        }
        catch(BusinessException<RegistrarAvaliadorDTO> b){
            return BadRequest(b.messages);
        }
        catch (Exception e)
        {
            return Problem("Algo deu errado");
        }
    }
    /// <summary>
    /// Realiza o login de um usuário.
    /// </summary>
    /// <param name="userLoginDTO">As credenciais do usuário para efetuar o login.</param>
    /// <returns>As informações de login do usuário.</returns>
    /// <remarks>
    /// Exemplo de requisição:
    ///
    ///     POST /login
    ///     {
    ///        "email": "joao.silva@example.com",
    ///        "senha": "SenhaSegura123"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Retorna as informações de login do usuário.</response>
    /// <response code="401">Se as credenciais do usuário forem inválidas.</response>
    /// <response code="500">Se ocorrer um erro inesperado.</response>
    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioLogadoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(UsuarioLoginDTO userLoginDTO)
    {
        try
        {
            var login = await _usuarioService.Login(userLoginDTO);
            return Ok(login);
        }
        catch(UnauthorizedException e){
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            return Problem("Algo deu errado");
        }
    }
}
