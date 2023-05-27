using AutoMapper;
using ploomes_teste.application.DTOs.Usuario;
using ploomes_teste.domain;

namespace ploomes_teste.application.Helpers
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile(){
            CreateMap<Usuario,UsuarioDTO>();
        }
    }
}