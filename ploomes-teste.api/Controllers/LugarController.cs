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
        public LugarController(ILugarService lugarService){
            _lugarService = lugarService;
        }
        
        [Authorize(Roles = "Proprietario")]
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarLugar(CriarLugarDTO criarLugarDTO){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarCriado = await _lugarService.CriarLugar(criarLugarDTO,user);
                return Ok(lugarCriado);
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

                var lugarAtualizar = await _lugarService.AtualizarLugar(atualizarLugarDTO,idLugar,user);
                return Ok(lugarAtualizar);
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
        [Route("page/{pageNumber}")]
        public async Task<IActionResult> GetLugaresPage(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarPage = await _lugarService.GetLugaresPage(pageNumber);
                return Ok(lugarPage);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Proprietario")]
        [HttpGet]
        [Route("proprietario/page/{pageNumber}")]
        public async Task<IActionResult> GetLugaresPageByUsuario(int pageNumber,string idUsuario){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarPage = await _lugarService.GetLugaresPageByUsuario(user,pageNumber);
                return Ok(lugarPage);
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

                var lugarDeletado = await _lugarService.DeletarLugar(idLugar,user);
                return Ok(lugarDeletado);
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
    }
}