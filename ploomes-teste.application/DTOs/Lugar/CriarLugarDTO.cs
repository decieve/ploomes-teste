using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.application.DTOs.Lugar
{
    public class CriarLugarDTO
    {
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public double Latitude {get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]

        public double Longitude {get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]

        public string NomeLugar {get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public string Cnpj{get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public int TipoLugar{get;set;}
    }
}