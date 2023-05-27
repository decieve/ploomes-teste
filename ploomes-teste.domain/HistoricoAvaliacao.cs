using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.domain
{
    public class HistoricoAvaliacao
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public double NotaAmbiente { get; set; }
        [Required]
        public double NotaAtendimento {get;set;}
        [Required]
        public double NotaQualidade {get;set;}
        [Required]
        public double NotaPreco {get;set;}
        public string Descricao {get;set;}
        [Required]
        public bool Anonimo {get;set;}
        [Required]
        public DateTime DataModificacao{get;set;}
        [Required]
        public bool Deletado{get;set;}
        [Required]
        public virtual Avaliacao AvaliacaoAtual { get; set; }
    }
}