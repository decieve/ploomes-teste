using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class CriarLugarNegocio : ICriarLugarNegocio
    { 
     
        private ILugarRepository _lugarRepository {get;set;}
        private ITipoLugarRepository _tipoLugarRepository {get;set;}
        public CriarLugarNegocio(ILugarRepository lugarRepository, ITipoLugarRepository tipoLugarRepository){
            _lugarRepository = lugarRepository;
            _tipoLugarRepository = tipoLugarRepository;
        }
        public bool ValidateCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] {5,4,3,2,9,8,7,6,5,4,3,2};
			int[] multiplicador2 = new int[13] {6,5,4,3,2,9,8,7,6,5,4,3,2};
			int soma;
			int resto;
			string digito;
			string tempCnpj;

			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

			if (cnpj.Length != 14)
			   return false;

			tempCnpj = cnpj.Substring(0, 12);

			soma = 0;
			for(int i=0; i<12; i++)
			   soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

			resto = (soma % 11);
			if ( resto < 2)
			   resto = 0;
			else
			   resto = 11 - resto;

			digito = resto.ToString();

			tempCnpj = tempCnpj + digito;
			soma = 0;
			for (int i = 0; i < 13; i++)
			   soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

			resto = (soma % 11);
			if (resto < 2)
			    resto = 0;
			else
			   resto = 11 - resto;

			digito = digito + resto.ToString();

			return cnpj.EndsWith(digito);
        }

        public async Task<bool> ValidateCnpjDuplicado(string cnpj)
        {
            try{
                var lugarDuplicado =  await _lugarRepository.GetLugarByCnpj(cnpj);
                return lugarDuplicado == null;
            }catch(Exception e){
                throw e;
            }
        }

        public bool ValidateLatitude(double latitude)
        {
            return latitude <= 90.0 && latitude >= -90.0;
        }

        public bool ValidateLongitude(double longitude)
        {
            return longitude <= 180.0 && longitude >= -180.0;
        }

        public async Task<bool> ValidateTipoLugar(short IdTipoLugar)
        {
            try{
                var tipoLugar = await _tipoLugarRepository.GetTipoLugarById(IdTipoLugar);
                return tipoLugar != null;
            }catch(Exception e){
                throw e;
            }
        }

        public async Task<List<string>> Validate(Lugar lugar, string idUsuarioLogado)
        {
            List<string> validationResult =new();

            if (!ValidateCnpj(lugar.Cnpj))
                validationResult.Add("CPNJ inválido");
          
            if (!ValidateLatitude(lugar.Latitude))
                validationResult.Add("A latitude não está entre 90 e -90 graus");
            
            if (!ValidateLongitude(lugar.Longitude))
               validationResult.Add("A longitude não está entre -180 e 180 graus");
            
            if (! await ValidateCnpjDuplicado(lugar.Cnpj))
                validationResult.Add("Já existe um lugar com o cnpj especificado");

            if(! await ValidateTipoLugar(lugar.TipoLugarId))
                validationResult.Add("O tipo de lugar especificado não existe.");

            return validationResult;
        }
    }
}