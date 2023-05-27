using AutoMapper;
using ploomes_teste.application.DTOs.Avaliacao;
using ploomes_teste.domain;

namespace ploomes_teste.application.Helpers
{
    public class AvaliacaoProfile : Profile
    {
        public AvaliacaoProfile()
        {
            CreateMap<Avaliacao, AvaliacaoUsuarioDTO>();
            CreateMap<Avaliacao,AvaliacaoLugarDTO>()
                .ForMember(d => d.NomeAvaliador,
                        map => map.MapFrom(i => i.Anonimo ? "Anônimo" : i.Usuario.NomeCompleto));
            CreateMap<CriarAtualizarAvaliacaoDTO, Avaliacao>();
            CreateMap<HistoricoAvaliacao,HistoricoAvaliacaoDTO>();
        }
    }
}