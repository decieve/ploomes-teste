using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class AlterarLugarNegocio : IAlterarLugarNegocio
    {
        private ILugarRepository _lugarRepository {get;set;}
        public AlterarLugarNegocio(ILugarRepository lugarRepository){
            _lugarRepository = lugarRepository;
        }
      
      
        public bool ValidateLatitude(double latitude)
        {
            return latitude <= 90.0 && latitude >= -90.0;
        }

        public bool ValidateLongitude(double longitude)
        {
            return longitude <= 180.0 && longitude >= 0.0;
        }

        public async Task<List<string>> Validate(Lugar lugar,string idUsuarioLogado)
        {
            List<string> validationResult =new();

           
          
            if (!ValidateLatitude(lugar.Latitude))
                validationResult.Add("A latitude não está entre 90 e -90 graus");
            
            if (!ValidateLatitude(lugar.Longitude))
               validationResult.Add("A longitude não está entre 0 e 180 graus");

            return validationResult;
        }
    }
}