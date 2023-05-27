using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ploomes_teste.domain
{
    public class Lugar
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id {get;set;}
        [Required]
        public double Latitude {get;set;}
        [Required]
        public double Longitude {get;set;}
        [Required,MaxLength(100)]
        public string NomeLugar {get;set;}
        [Required]
        public bool Deletado{get;set;}
        [Required]
        public string Cnpj{get;set;}
        [Required]
        public string ProprietarioId { get; set; }
        [Required]
        public short TipoLugarId{get;set;}
        [Required]
        public virtual TipoLugar TipoLugar {get;set;}
        [Required]
        public virtual Usuario Proprietario {get;set;}
        public virtual ICollection<Avaliacao> Avaliacoes{get;set;}
    }
}