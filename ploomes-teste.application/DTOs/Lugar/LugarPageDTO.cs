namespace ploomes_teste.application.DTOs.Lugar
{
    public class LugarPageDTO
    {
        public LugarDTO[] Lugares{get;set;}
        public int PageNumber {get;set;}
        public int PageSize {get;set;}
    }
}