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

    [Route("get")]
    [HttpGet]
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

    [AllowAnonymous]
    [Route("registrar/proprietario")]
    [HttpPost]
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

    [AllowAnonymous]
    [Route("registrar/avaliador")]
    [HttpPost]
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

    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
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
