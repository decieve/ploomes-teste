using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class RegistrarAvaliadorNegocio : IRegistrarAvaliadorNegocio
    {
        public bool ValidateLatitude(double latitude)
        {
            return latitude <= 90.0 && latitude >= -90.0;
        }

        public bool ValidateLongitude(double longitude)
        {
            return longitude <= 180.0 && longitude >= -180.0;
        }

        public List<string> Validate(Usuario usuario)
        {
            List<string> validationResult =new();

           
          
            if (!ValidateLatitude(usuario.LatitudeMoradia.Value))
                validationResult.Add("A latitude não está entre 90 e -90 graus");
            
            if (!ValidateLatitude(usuario.LongitudeMoradia.Value))
               validationResult.Add("A longitude não está entre -180 e 180 graus");

            return validationResult;
        }
    }
}