using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.application.DTOs.Avaliacao
{
    public class AtualizarAvaliacaoDTO
    {
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public double NotaAmbiente { get; set; }
       
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public double NotaAtendimento {get;set;}
    
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public double NotaQualidade {get;set;}

        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public double NotaPreco {get;set;}

        [MaxLength(500,ErrorMessage = "A descrição não pode ter mais que 500 caracteres")]
        public string? Descricao {get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public bool Anonimo {get;set;}
    }
}