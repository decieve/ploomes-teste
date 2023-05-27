using ploomes_teste.domain;

namespace ploomes_teste.negocio.Contracts
{
    public interface IRegistrarAvaliadorNegocio
    {
        bool ValidateLatitude(double latitude);
        bool ValidateLongitude(double longitude);
        List<string> Validate(Usuario usuario);
    }
}