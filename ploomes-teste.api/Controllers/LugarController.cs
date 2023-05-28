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

        /// <summary>
        /// Cria um Lugar.
        /// </summary>
        /// <param name="criarLugarDTO">Os dados para criar um Lugar.</param>
        /// <returns>O Lugar recém-criado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /lugar/criar
        ///     {
	    ///         "cnpj":"39198939000148",
        ///         "latitude":1,
        ///         "longitude":1,
        ///         "nomeLugar":"Bar do julião",
        ///         "tipoLugar":3
        ///     }
        /// </remarks>
        /// <response code="200">Retorna o Lugar recém-criado.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou possuir um token inválido no header).</response>
        /// <response code="403">Se o usuário não for um Proprietário.</response>
        /// <response code="400">Se houver um erro de validação.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Proprietario")]
        [HttpPost]
        [Route("criar")]
        [ProducesResponseType(typeof(LugarDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Atualiza um Lugar.
        /// </summary>
        /// <param name="atualizarLugarDTO">Os dados para atualizar um Lugar.</param>
        /// <param name="idLugar">O ID do Lugar a ser atualizado.</param>
        /// <returns>O Lugar atualizado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     PUT /lugar/atualizar/afa90766-6dc2-470e-0d0c-08db5eeb260f
        ///     {
	    ///         "cnpj":"39198939000148",
        ///         "latitude":1,
        ///         "longitude":1,
        ///         "nomeLugar":"Bar do julião",
        ///         "tipoLugar":3
        ///     }
        /// </remarks>
        /// <response code="200">Retorna o Lugar atualizado.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou possuir um token inválido no header).</response>
        /// <response code="403">Se o usuário não for um Proprietário.</response>
        /// <response code="404">Se o Lugar não for encontrado.</response>
        /// <response code="400">Se houver um erro de validação.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Proprietario")]
        [HttpPut]
        [Route("atualizar/{idLugar}")]
        [ProducesResponseType(typeof(LugarDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Obtém uma página de Lugares.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser obtida.</param>
        /// <returns>Uma página de Lugares.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /lugar/page/1
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de Lugares.</response>
        /// <response code="400">Se o número da página for inválido.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("page/{pageNumber}")]
        [ProducesResponseType(typeof(LugarPageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLugaresPage(int pageNumber)
        {
            try
            {
                var lugarPage = await _lugarService.GetLugaresPage(pageNumber);
                return Ok(lugarPage);
            }
            catch (InvalidPageException e)
            {
                return BadRequest(new string[] { e.Message });
            }
            catch (Exception e)
            {
                return Problem("Algo deu errado");
            }
        }
        /// <summary>
        /// Obtém uma página de Lugares de um Proprietário.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser obtida.</param>
        /// <returns>Uma página de Lugares pertencentes ao Proprietário autenticado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /lugar/proprietario/page/1
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de Lugares do Proprietário autenticado.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou possuir um token inválido no header).</response>
        /// <response code="400">Se o número da página for inválido.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Proprietario")]
        [HttpGet]
        [Route("proprietario/page/{pageNumber}")]
        [ProducesResponseType(typeof(LugarPageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Obtém uma página de Lugares com base na localização do Avaliador.
        /// </summary>
        /// <param name="pageNumber">O número da página a ser obtida.</param>
        /// <returns>Uma página de Lugares com base na localização do Avaliador autenticado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /lugar/avaliador/localizacao/page/1
        ///
        /// </remarks>
        /// <response code="200">Retorna a página de Lugares com base na localização do Avaliador autenticado.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou possuir um token inválido no header).</response>
        /// <response code="400">Se o número da página for inválido.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Avaliador")]
        [HttpGet]
        [Route("avaliador/localizacao/page/{pageNumber}")]
        [ProducesResponseType(typeof(LugarPageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Deleta um Lugar.
        /// </summary>
        /// <param name="idLugar">O ID do Lugar a ser deletado.</param>
        /// <returns>O Lugar que foi deletado.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     DELETE /lugar/deletar/ffb47858-74b0-49e7-2e08-08db5eef9d96
        ///
        /// </remarks>
        /// <response code="200">Retorna o Lugar que foi deletado.</response>
        /// <response code="401">Se o usuário não estiver autenticado (ou possuir um token inválido no header).</response>
        /// <response code="404">Se o Lugar não for encontrado.</response>
        /// <response code="403">Se o usuário não for o Proprietário do Lugar.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [Authorize(Roles = "Proprietario")]
        [HttpDelete]
        [Route("deletar/{idLugar}")]
        [ProducesResponseType(typeof(LugarDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
        /// <summary>
        /// Obtém os tipos de lugares disponíveis.
        /// </summary>
        /// <returns>Uma lista dos tipos de lugares.</returns>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     GET /lugar/tipos
        ///
        /// </remarks>
        /// <response code="200">Retorna uma lista dos tipos de lugares.</response>
        /// <response code="500">Se ocorrer um erro inesperado.</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("tipos")]
        [ProducesResponseType(typeof(TipoLugarDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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