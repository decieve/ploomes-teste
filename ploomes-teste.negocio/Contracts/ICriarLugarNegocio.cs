using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;

namespace ploomes_teste.negocio.Contracts
{
    public interface ICriarLugarNegocio
    {
        bool ValidateLatitude(double latitude);
        bool ValidateLongitude(double longitude);
        bool ValidateCnpj(string cnpj);
        Task<bool> ValidateCnpjDuplicado(string cnpj);
        Task<bool> ValidateTipoLugar(short IdTipoLugar);
        Task<List<string>> Validate(Lugar lugar,string idUsuarioLogado);

    }
}