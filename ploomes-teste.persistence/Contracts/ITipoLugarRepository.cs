using ploomes_teste.domain;

namespace ploomes_teste.persistence.Contracts
{
    public interface ITipoLugarRepository : IGeneralRepository
    {
        Task<TipoLugar> GetTipoLugarById(short id);
        Task<TipoLugar[]> GetTiposLugar();
    }
}