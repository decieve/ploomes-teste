using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ploomes_teste.domain
{
    public class Avaliacao {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [MaxLength(500)]
        public string? Descricao {get;set;}
        [Required]
        public bool Anonimo {get;set;}
        [Required]
        public bool Deletado{get;set;}
        [Required]
        public DateTime DataPostada {get;set;}
        [Required]
        public DateTime DataAtualizada {get;set;}
        [Required]
        public string AvaliadorId{get;set;}
        [Required]
        public Guid LugarId{get;set;}
        
        [Required]
        public virtual Usuario Avaliador {get;set;}
        [Required]
        public virtual Lugar Lugar { get; set; }
        public virtual ICollection<HistoricoAvaliacao> Historico {get;set;} 
        
    }
}