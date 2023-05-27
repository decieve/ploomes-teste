using AutoMapper;
using ploomes_teste.application.DTOs.Lugar;
using ploomes_teste.domain;

namespace ploomes_teste.application.Helpers
{
    public class LugarProfile : Profile
    {
        public LugarProfile()
        {
            CreateMap<Lugar, LugarDTO>()
                .ForMember(l => l.TipoLugar,
                    map => map.MapFrom(l => l.TipoLugar.Nome));
            CreateMap<CriarLugarDTO, Lugar>();
            CreateMap<AtualizarLugarDTO, Lugar>();
            CreateMap<TipoLugar,TipoLugarDTO>();
        }
    }
}