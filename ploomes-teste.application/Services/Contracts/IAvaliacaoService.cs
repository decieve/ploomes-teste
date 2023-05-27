using ploomes_teste.application.DTOs.Avaliacao;
using ploomes_teste.domain;

namespace ploomes_teste.application.Services.Contracts
{
    public interface IAvaliacaoService
    {
        Task<AvaliacaoUsuarioDTO> CriarAvaliacao(CriarAvaliacaoDTO avaliacao,Usuario usuarioLogado);
        Task<AvaliacaoUsuarioDTO> AtualizarAvaliacao(AtualizarAvaliacaoDTO avaliacaoAtualizada,Guid idAvaliacao,Usuario usuarioLogado);
        Task<AvaliacaoUsuarioDTO> DeletarAvaliacao(Guid idAvaliacao,Usuario usuarioLogado);
        Task<AvaliacaoLugarPageDTO> GetAvaliacoesPageByLugar(Guid idLugar,int pageNumber,int pageSize = 10,bool includeHistorico = false);
        Task<AvaliacaoUsuarioPageDTO> GetAvaliacoesPageByAvaliador(Usuario usuario,int pageNumber,int pageSize = 10,bool includeHistorico = false);
        Task<AvaliacaoUsuarioDTO> GetAvaliacaoById(Guid idAvaliacao,bool includeHistorico = true);
        
    }
}