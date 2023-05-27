namespace ploomes_teste.persistence.Contracts
{
    public interface ITipoLugarRepository : IGeneralRepository
    {
        Task<TipoLugar> GetTipoLugarById(int id);
        Task<TipoLugar[]> GetTiposLugar();
    }
}