namespace ploomes_teste.application.DTOs.Avaliacao
{
    public class AvaliacaoUsuarioPageDTO
    {

        public AvaliacaoUsuarioDTO[] Avaliacoes{get;set;}
        public int PageNumber {get;set;}
        public int PageSize {get;set;}
    }
}