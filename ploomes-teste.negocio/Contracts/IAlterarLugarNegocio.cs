using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;

namespace ploomes_teste.negocio.Contracts
{
    public interface IAlterarLugarNegocio 
    {
        
        bool ValidateLatitude(double latitude);
        bool ValidateLongitude(double longitude);
        Task<List<string>> Validate(Lugar lugar,string idUsuarioLogado);
    }
}