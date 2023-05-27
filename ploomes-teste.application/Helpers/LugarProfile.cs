using AutoMapper;
using ploomes_teste.application.DTOs.Lugar;
using ploomes_teste.domain;
using ploomes_teste.domain.helpers;

namespace ploomes_teste.application.Helpers
{
    public class LugarProfile : Profile
    {
        public LugarProfile()
        {
            CreateMap<Lugar, LugarDTO>()
                .ForMember(l => l.TipoLugar,
                    map => map.MapFrom(l => l.TipoLugar.Nome));


            CreateMap<LugarDistancia, LugarDTO>()
                .ForMember(l => l.Avaliacoes,
                    map => map.MapFrom(l => l.Lugar.Avaliacoes))
                .ForMember(l => l.Cnpj,
                    map => map.MapFrom(l => l.Lugar.Cnpj))
                .ForMember(l => l.Id,
                    map => map.MapFrom(l => l.Lugar.Id))
                .ForMember(l => l.Latitude,
                    map => map.MapFrom(l => l.Lugar.Latitude))
                .ForMember(l => l.Longitude,
                    map => map.MapFrom(l => l.Lugar.Longitude))
                .ForMember(l => l.NomeLugar,
                    map => map.MapFrom(l => l.Lugar.NomeLugar))
                .ForMember(l => l.DistanciaAproximada,
                    map => map.MapFrom(l => l.Distancia))
                .ForMember(l => l.TipoLugar,
                    map => map.MapFrom(l => l.Lugar.TipoLugar.Nome));



            CreateMap<CriarLugarDTO, Lugar>()
                .ForMember(l => l.TipoLugar,
                    map => map.Ignore())
                .ForMember(l => l.TipoLugarId,
                    map => map.MapFrom(m => m.TipoLugar));
            CreateMap<AtualizarLugarDTO, Lugar>();
            CreateMap<TipoLugar,TipoLugarDTO>();
        }
    }
}