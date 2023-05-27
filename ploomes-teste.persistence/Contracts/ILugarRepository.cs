using ploomes_teste.domain;

namespace ploomes_teste.persistence.Contracts
{
    public interface ILugarRepository : IGeneralRepository
    {
        Task<Lugar> GetLugarById(Guid id);
        Task<Lugar[]> GetLugaresPage(int pageNumber,int pageSize = 10);
        Task<Lugar[]> GetLugaresByUsuario(int pageNumber,string idUsuario,int pageSize = 10);
        Task<Lugar> GetLugarByCnpj(string Cnpj);
    }
}