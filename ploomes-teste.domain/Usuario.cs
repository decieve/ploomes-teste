using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ploomes_teste.domain
{

    // Existem dois tipos de usuario, proprietário e avaliador, todos eles tem os mesmos dados mas são diferenciados pela role, e cada dado é particular de um usuario
    public class Usuario : IdentityUser
    {
        // Apenas disponivel para Proprietarios
        public virtual ICollection<Lugar> Lugares{get;set;}
        // Apenas disponivel para Avaliadores
        public virtual ICollection<Avaliacao> Avaliacoes { get; set; }
        // Apenas disponivel para Avaliadores
        public double LatitudeMoradia {get;set;}
        // Apenas disponivel para Avaliadores
        public double LongitudeMoradia {get;set;}
        [Required]
        public string NomeCompleto{get;set;}
        [Required]
        public bool Deletado{get;set;}
    }
}