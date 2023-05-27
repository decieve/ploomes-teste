using System.ComponentModel.DataAnnotations;

namespace ploomes_teste.application.DTOs.Usuario
{
    public class RegistrarUsuarioDTO
    {
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        [MinLength(4,ErrorMessage = "O nome de usuário precisa ter 4 ou mais caracteres. (possui {1})")]
        [MaxLength(20,ErrorMessage = "O nome de usuário não pode ter mais que 20 caracteres. (possui {1})")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "O nome de usuário não pode conter caracteres especiais.")]
        public string UserName {get;set;}


        [Required(ErrorMessage = "É necessário preencher o campo email.")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "O email não pode conter caracteres especiais.")]
        public string Email {get;set;}


        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        [MinLength(4,ErrorMessage = "O nome completo precisa ter mais que 4 caracteres. (possui {1})")]
        [MaxLength(100,ErrorMessage = "O nome completo não pode ter mais que 100 caracteres. (possui {1})")]
        [RegularExpression(@"^[a-zA-Z0-9áãàâéèêíìîóõòôúùû]+$", ErrorMessage = "O nome completo possui caracteres não aceitos.")]
        public string NomeCompleto {get;set;}

        public double? Latitude {get;set;}

        public double? Longitude{get;set;}
        [Required(ErrorMessage = "É necessário preencher o campo {0}.")]
        [MinLength(4,ErrorMessage = "A senha precisa ter 4 ou mais caracteres. (possui {1})")]
        [MaxLength(64,ErrorMessage = "A senha não pode ter mais que 64 caracteres. (possui {1})")]
        public string Password{get;set;}
    }
}