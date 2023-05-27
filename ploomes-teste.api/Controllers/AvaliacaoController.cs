using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ploomes_teste.application.DTOs.Avaliacao;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.domain;

namespace ploomes_teste.api.Controllers;

[ApiController]
[Route("[controller]")]
public class AvaliacaoController : ControllerBase
{
        private readonly IAvaliacaoService _avaliacaoService;
        private readonly UserManager<Usuario> _userManager;
        public AvaliacaoController(IAvaliacaoService avaliacaoService,UserManager<Usuario> userManager){
            _avaliacaoService = avaliacaoService;
            _userManager =userManager;
        }
        
        [Authorize(Roles = "Avaliador")]
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarAvaliacao(CriarAvaliacaoDTO criarAvaliacaoDTO){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarCriado = await _avaliacaoService.CriarAvaliacao(criarAvaliacaoDTO,user);
                return Ok(lugarCriado);
            }
            catch(BusinessException<CriarAvaliacaoDTO> e){
                return BadRequest(e.messages);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Avaliador")]
        [HttpPut]
        [Route("atualizar/{idAvaliacao}")]
        public async Task<IActionResult> AtualizarAvaliacao(AtualizarAvaliacaoDTO atualizarAvaliacaoDTO,Guid idAvaliacao){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                
                var avaliacaoAtualizar = await _avaliacaoService.AtualizarAvaliacao(atualizarAvaliacaoDTO,idAvaliacao,user);
                return Ok(avaliacaoAtualizar);
            }
            catch(BusinessException<AtualizarAvaliacaoDTO> e){
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
        [Authorize(Roles = "Avaliador")]
        [HttpGet]
        [Route("avaliador/page/{pageNumber}")]
        public async Task<IActionResult> GetAvaliacoesPageByAvaliador(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var avaliacoesPage = await _avaliacaoService.GetAvaliacoesPageByAvaliador(user,pageNumber);
                return Ok(avaliacoesPage);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("lugar/{idLugar}/page/{pageNumber}")]
        public async Task<IActionResult> GetAvaliacoesPageByLugar(int pageNumber,Guid idLugar){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarPage = await _avaliacaoService.GetAvaliacoesPageByLugar(idLugar,pageNumber);
                return Ok(lugarPage);
            }
            
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Avaliador")]
        [HttpDelete]
        [Route("deletar/{idAvaliacao}")]
        public async Task<IActionResult> DeletarAvaliacao(Guid idAvaliacao){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var avaliacaoDeletada = await _avaliacaoService.DeletarAvaliacao(idAvaliacao,user);
                return Ok(avaliacaoDeletada);
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
