using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.domain
{
    public class Lugar
    {
        [Key]
        public Guid Uuid {get;set;}
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
        public virtual ICollection<Avaliacao> Avaliacoes{get;set;}
    }
}