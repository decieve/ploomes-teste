using ploomes_teste.domain;

namespace ploomes_teste.persistence.Contracts
{
    public interface IAvaliacaoRepository : IGeneralRepository
    {
        Task<Avaliacao> GetAvaliacaoById(Guid id,bool includeHistorico = true);
        Task<Avaliacao[]> GetAvaliacoesByLugarPage(int pageNumber,Guid idLugar,int pageSize = 10,bool includeHistorico = false);
        Task<Avaliacao[]> GetAvaliacoesByUsuario(int pageNumber,string id_usuario,int pageSize = 10,bool includeHistorico = false);
        Task<Avaliacao> GetAvaliacaoByIdLugarIdUsuario(Guid idLugar,string idUsuario,bool includeHistorico = false);
    }
}