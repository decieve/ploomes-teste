namespace ploomes_teste.application.DTOs.Avaliacao
{
    public class AvaliacaoLugarDTO
    {
        
        public Guid Id { get; set; }
       
        public double NotaAmbiente { get; set; }
       
        public double NotaAtendimento {get;set;}
     
        public double NotaQualidade {get;set;}
       
        public double NotaPreco {get;set;}
        
        public string Descricao {get;set;}

        public DateTime DataPostada {get;set;}
     
        public DateTime DataAtualizada {get;set;}
        // "Anonimo" se escolheu uma avaliacao anonima
        public string NomeAvaliador {get;set;}
        
        public List<HistoricoAvaliacaoDTO> Historico {get;set;} 
    }
}