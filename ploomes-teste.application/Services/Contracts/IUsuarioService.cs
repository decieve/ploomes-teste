using System.Security.Claims;
using ploomes_teste.application.DTOs.Usuario;
using ploomes_teste.domain;

namespace ploomes_teste.application.Services.Contracts
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> GetUser(ClaimsPrincipal principal);
        Task<UsuarioDTO> RegistrarProprietario(RegistrarProprietarioDTO userDTO);
        Task<UsuarioDTO> RegistrarAvaliador(RegistrarAvaliadorDTO userDTO);
        Task<UsuarioLogadoDTO> Login(UsuarioLoginDTO userLoginDTO);

    }
}