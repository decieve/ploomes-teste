using ploomes_teste.application.DTOs.Lugar;
using ploomes_teste.domain;

namespace ploomes_teste.application.Services.Contracts
{
    public interface ILugarService
    {
         
        Task<LugarDTO> CriarLugar(CriarLugarDTO lugar,Usuario usuarioLogado);
        Task<LugarDTO> AtualizarLugar(AtualizarLugarDTO lugarAtualizado,Guid IdLugar,Usuario usuarioLogado);
        Task<LugarDTO> DeletarLugar(Guid idLugar,Usuario usuarioLogado);
        Task<LugarPageDTO> GetLugaresPage(int pageNumber,int pageSize = 10);
        Task<LugarPageDTO> GetLugaresPageByProprietario(Usuario usuarioLogado,int pageNumber,int pageSize = 10);    
        Task<LugarDTO> GetLugarById(Guid idLugar);
        Task<LugarPageDTO> GetLugaresPageByLocalizacaoAvaliador(Usuario usuarioLogado,int pageNumber,int pageSize = 10);
        Task<TipoLugarDTO[]> GetTiposLugares();
    }
}