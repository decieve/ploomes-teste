using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ploomes_teste.application.DTOs.Lugar;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.domain;

namespace ploomes_teste.api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class LugarController : ControllerBase
    {
        private readonly ILugarService _lugarService;
        private readonly UserManager<Usuario> _userManager;
        public LugarController(ILugarService lugarService,UserManager<Usuario> userManager){
            _lugarService = lugarService;
            _userManager = userManager;
        }
        
        [Authorize(Roles = "Proprietario")]
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarLugar(CriarLugarDTO criarLugarDTO){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarCriado = await _lugarService.CriarLugar(criarLugarDTO,user);
                return Ok(lugarCriado);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(BusinessException<CriarLugarDTO> e){
                return BadRequest(e.messages);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Proprietario")]
        [HttpPut]
        [Route("atualizar/{idLugar}")]
        public async Task<IActionResult> AtualizarLugar(AtualizarLugarDTO atualizarLugarDTO,Guid idLugar){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarAtualizar = await _lugarService.AtualizarLugar(atualizarLugarDTO,idLugar,user);
                return Ok(lugarAtualizar);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(BusinessException<AtualizarLugarDTO> e){
                return BadRequest(e.messages);
            }
            catch(NotFoundException e){
                return NotFound(e.Message);
            }
            catch(ForbiddenException e){
                return new ObjectResult(e.Message) { StatusCode = 403};
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("page/{pageNumber}")]
        public async Task<IActionResult> GetLugaresPage(int pageNumber){
            try{
                var lugarPage = await _lugarService.GetLugaresPage(pageNumber);
                return Ok(lugarPage);
            }
            catch(InvalidPageException e){
                return BadRequest(new string[]{e.Message});
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Proprietario")]
        [HttpGet]
        [Route("proprietario/page/{pageNumber}")]
        public async Task<IActionResult> GetLugaresPageByProprietario(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarPage = await _lugarService.GetLugaresPageByProprietario(user,pageNumber);
                return Ok(lugarPage);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(InvalidPageException e){
                return BadRequest(new string[]{e.Message});
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Avaliador")]
        [HttpGet]
        [Route("avaliador/localizacao/page/{pageNumber}")]
        public async Task<IActionResult> GetLugaresPageByLocalizacaoAvaliador(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarPage = await _lugarService.GetLugaresPageByLocalizacaoAvaliador(user,pageNumber);
                return Ok(lugarPage);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(InvalidPageException e){
                return BadRequest(new string[]{e.Message});
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Proprietario")]
        [HttpDelete]
        [Route("deletar/{idLugar}")]
        public async Task<IActionResult> CriarLugar(Guid idLugar){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarDeletado = await _lugarService.DeletarLugar(idLugar,user);
                return Ok(lugarDeletado);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(NotFoundException e){
                return NotFound(e.Message);
            }
            catch(ForbiddenException e){
                return Forbid(e.Message);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("tipos")]
        public async Task<IActionResult> GetTiposLugares(){
            try{
                var tiposLugares = await _lugarService.GetTiposLugares();
                return Ok(tiposLugares);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
    }
}