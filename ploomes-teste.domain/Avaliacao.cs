using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ploomes_teste.domain
{
    public class Avaliacao {
        [Key]
        public Guid Uuid { get; set; }
        [Required]
        public double NotaAmbiente { get; set; }
        [Required]
        public double NotaAtendimento {get;set;}
        [Required]
        public double NotaQualidade {get;set;}
        [Required]
        public double NotaPreco {get;set;}
        [MaxLength(500)]
        public string Descricao {get;set;}
        [Required]
        public bool Anonimo {get;set;}
        [Required]
        public bool Deletado{get;set;}
        [Required]
        public DateTime DataPostada {get;set;}
        public DateTime DataAtualizada {get;set;}
        [Required]
        public virtual Usuario Usuario {get;set;}
        [Required]
        public virtual Lugar Lugar { get; set; }
        public virtual ICollection<HistoricoAvaliacao> Historico {get;set;} 
        
    }
}