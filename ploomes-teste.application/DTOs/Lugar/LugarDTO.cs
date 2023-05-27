using ploomes_teste.application.DTOs.Avaliacao;

namespace ploomes_teste.application.DTOs.Lugar
{
    public class LugarDTO
    {
        public Guid Id {get;set;}

        public double Latitude {get;set;}

        public double Longitude {get;set;}

        public string NomeLugar {get;set;}

        public string Cnpj{get;set;}

        public double NotaMedia{get;set;}
        public string TipoLugar{get;set;}
        public virtual AvaliacaoLugarDTO[] Avaliacoes{get;set;}
    }
}