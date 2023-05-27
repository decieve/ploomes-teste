namespace ploomes_teste.application.DTOs.Avaliacao
{
    public class HistoricoAvaliacaoDTO
    {
       
        public Guid Id { get; set; }
      
        public double NotaAmbiente { get; set; }
       
        public double NotaAtendimento {get;set;}
      
        public double NotaQualidade {get;set;}
      
        public double NotaPreco {get;set;}
        public string Descricao {get;set;}
      
        public DateTime DataModificacao{get;set;}
    }
}