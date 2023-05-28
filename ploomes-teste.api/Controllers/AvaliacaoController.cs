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
        /// <summary>
        ///     Cria uma Avaliação.
        /// </summary>
        /// <param name="criarAvaliacaoDTO">Os dados para criar uma Avaliação.</param>
        /// <returns>A Avaliação recém-criada.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /avaliacao/criar
        ///     {
        ///        "notaPreco":1.3,
        ///        "notaAmbiente":2.4,
        ///        "notaQualidade":3,
        ///        "notaAtendimento":4,
        ///        "descricao":"Foi legal, porém caro e o ambiente não era muito bom.",
        ///        "anonimo":true,
        ///         "lugarId":"bc72d692-2057-4418-6a75-08db5ef0b884"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna a Avaliação recém-criada.</response>
        /// <response code="401">Se o usuário não estiver autenticado (possuir um token no header).</response>
        /// <response code="403">Se o usuário for um Proprietário.</response>
        /// <response code="400">Se houver um erro de validação.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Avaliador")]
        [HttpPost]
        [Route("criar")]
        [ProducesResponseType(typeof(AvaliacaoUsuarioDTO),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized )]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden )]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest )]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError )]
        public async Task<IActionResult> CriarAvaliacao(CriarAvaliacaoDTO criarAvaliacaoDTO){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var lugarCriado = await _avaliacaoService.CriarAvaliacao(criarAvaliacaoDTO,user);
                return Ok(lugarCriado);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
            }
            catch(BusinessException<CriarAvaliacaoDTO> e){
                return BadRequest(e.messages);
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        /// <summary>
        ///     Altera uma Avaliação.
        /// </summary>
        /// <param name="atualizarAvaliacaoDTO">Os dados para alterar uma Avaliação existente.</param>
        /// <param name="idAvaliacao">O id da avaliação a ser modificada</param>
        /// <returns>A Avaliação alterada.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /avaliacao/atualizar/afa90766-6dc2-470e-0d0c-08db5eeb260f
        ///     {
        ///        "notaPreco":3,
        ///        "notaAmbiente":3.4,
        ///        "notaQualidade":3,
        ///        "notaAtendimento":4,
        ///        "descricao":"Foi legal, porém caro e o ambiente não era muito bom.",
        ///        "anonimo":true
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Retorna a Avaliação recém-criada.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou não possuir um token válido no header).</response>
        /// <response code="403">Se o usuário for um proprietário, ou não for o usuário que realizou a avaliação.</response>
        /// <response code="400">Se houver um erro de validação.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Avaliador")]
        [HttpPut]
        [Route("atualizar/{idAvaliacao}")]
        [ProducesResponseType(typeof(AvaliacaoUsuarioDTO),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized )]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden )]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound )]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest )]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError )]
        public async Task<IActionResult> AtualizarAvaliacao(AtualizarAvaliacaoDTO atualizarAvaliacaoDTO,Guid idAvaliacao){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var avaliacaoAtualizar = await _avaliacaoService.AtualizarAvaliacao(atualizarAvaliacaoDTO,idAvaliacao,user);
                return Ok(avaliacaoAtualizar);
            }
            catch(UnauthorizedException e){
                return Unauthorized(e.Message);
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
        /// <summary>
        ///     Obtém as avaliações de um usuário logado
        /// </summary>
        /// <param name="pageNumber">O número da página</param>
        /// <returns>Retorna a página de avaliações especificada do usuário logado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /avaliador/page/1
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de avaliações .</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou não possuir um token válido no header).</response>
        /// <response code="403">Se o usuário for um proprietário</response>
        /// <response code="400">Se o número da página for menor que 1.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Avaliador")]
        [HttpGet]
        [Route("avaliador/page/{pageNumber}")]
        [ProducesResponseType(typeof(AvaliacaoUsuarioPageDTO),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized )]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden )]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest )]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError )]
        public async Task<IActionResult> GetAvaliacoesPageByAvaliador(int pageNumber){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var avaliacoesPage = await _avaliacaoService.GetAvaliacoesPageByAvaliador(user,pageNumber);
                return Ok(avaliacoesPage);
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
        /// <summary>
        ///     Obtém as avaliações de um lugar especificado
        /// </summary>
        /// <param name="pageNumber">O número da página</param>
        /// <param name="idLugar">O id do lugar</param>
        /// <returns>Retorna a página de avaliações de um lugar</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /avaliacao/lugar/7bf4e5c4-0595-4649-911d-08db5f0cbbd3/page/1
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de avaliações .</response>
        /// <response code="400">Se o número da página for menor que 1.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("lugar/{idLugar}/page/{pageNumber}")]
        [ProducesResponseType(typeof(AvaliacaoLugarPageDTO),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest )]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError )]
        public async Task<IActionResult> GetAvaliacoesPageByLugar(int pageNumber,Guid idLugar){
            try{

                var lugarPage = await _avaliacaoService.GetAvaliacoesPageByLugar(idLugar,pageNumber);
                return Ok(lugarPage);
            }
            catch(InvalidPageException e){
                return BadRequest(new string[]{e.Message});
            }
            catch(Exception e){
                return Problem("Algo deu errado");
            }
        }
        /// <summary>
        ///     Deleta uma avaliação especifiada através do id
        /// </summary>
        /// <param name="idAvaliacao">O id da avaliação a ser deletada</param>
        /// <returns>Retorna a avaliação deletada</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /avaliacao/deletar/7bf4e5c4-0595-4649-911d-08db5f0cbbd3/
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de avaliações .</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou não possuir um token válido no header).</response>
        /// <response code="403">Se o usuário for um proprietário ou não for o usuário que criou a avaliação</response>
        /// <response code="400">Se houver algum erro de validação.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Avaliador")]
        [HttpDelete]
        [Route("deletar/{idAvaliacao}")]
        [ProducesResponseType(typeof(AvaliacaoUsuarioDTO),StatusCodes.Status200OK )]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized )]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden )]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound )]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest )]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError )]
        public async Task<IActionResult> DeletarAvaliacao(Guid idAvaliacao){
            try{
                ClaimsPrincipal principal = HttpContext.User as ClaimsPrincipal;
                var user = await _userManager.GetUserAsync(principal);
                if(user == null)
                    throw new UnauthorizedException("Token inválido");
                var avaliacaoDeletada = await _avaliacaoService.DeletarAvaliacao(idAvaliacao,user);
                return Ok(avaliacaoDeletada);
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
}
