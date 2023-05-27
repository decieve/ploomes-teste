namespace ploomes_teste.application.DTOs.Avaliacao
{
    public class AvaliacaoLugarPageDTO
    {
        public AvaliacaoLugarDTO[] Avaliacoes{get;set;}
        public int PageNumber {get;set;}
        public int PageSize {get;set;}
    }
}