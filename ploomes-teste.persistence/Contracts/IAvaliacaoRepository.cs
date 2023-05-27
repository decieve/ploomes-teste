using ploomes_teste.domain;

namespace ploomes_teste.persistence.Contracts
{
    public interface IAvaliacaoRepository : IGeneralRepository
    {
        Task<Avaliacao> GetAvaliacaoById(Guid id,bool includeHistorico = true);
        Task<Avaliacao[]> GetAvaliacoesByLugarPage(int pageNumber,Guid idLugar,int pageSize = 10,bool includeHistorico = false);
        Task<Avaliacao[]> GetAvaliacoesByAvaliador(int pageNumber,string idAvaliador,int pageSize = 10,bool includeHistorico = false);
        Task<Avaliacao> GetAvaliacaoByIdLugarIdAvaliador(Guid idLugar,string idAvaliador,bool includeHistorico = false);
    }
}