using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.application.DTOs.Usuario
{
    public class UsuarioLoginDTO
    {
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public string UserName {get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        public string Password{get;set;}
    }
}