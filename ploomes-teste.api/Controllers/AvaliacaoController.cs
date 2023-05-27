using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public AvaliacaoController(IAvaliacaoService avaliacaoService){
            _avaliacaoService = avaliacaoService;
        }
        
        [Authorize(Roles = "Avaliador")]
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarAvaliacao(CriarAtualizarAvaliacaoDTO criarAvaliacaoDTO){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var lugarCriado = await _avaliacaoService.CriarAvaliacao(criarAvaliacaoDTO,user);
                return Ok(lugarCriado);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        [Authorize(Roles = "Avaliador")]
        [HttpPut]
        [Route("atualizar/{idAvaliacao}")]
        public async Task<IActionResult> AtualizarAvaliacao(CriarAtualizarAvaliacaoDTO atualizarAvaliacaoDTO,Guid idAvaliacao){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                
                var avaliacaoAtualizar = await _avaliacaoService.AtualizarAvaliacao(atualizarAvaliacaoDTO,idAvaliacao,user);
                return Ok(avaliacaoAtualizar);
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
        [Authorize(Roles = "Avaliador")]
        [HttpGet]
        [Route("avaliador/page/{pageNumber}")]
        public async Task<IActionResult> GetAvaliacoesPageByUsuario(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);

                var avaliacoesPage = await _avaliacaoService.GetAvaliacoesPageByUsuario(user,pageNumber);
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
