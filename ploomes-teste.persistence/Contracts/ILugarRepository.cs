using ploomes_teste.domain;
using ploomes_teste.domain.helpers;
using ploomes_teste.persistence.Implementations;

namespace ploomes_teste.persistence.Contracts
{
    public interface ILugarRepository : IGeneralRepository
    {
        Task<Lugar> GetLugarById(Guid id);
        Task<Lugar[]> GetLugaresPage(int pageNumber,int pageSize = 10);
        Task<Lugar[]> GetLugaresByProprietario(int pageNumber,string idUsuario,int pageSize = 10);
        Task<LugarDistancia[]> GetLugaresByDistanciaAvaliador(int pageNumber,double latitudeAvaliador,double longitudeAvaliador,int pageSize =10);
        Task<Lugar> GetLugarByCnpj(string Cnpj);
    }
}