using AutoMapper;
using ploomes_teste.application.DTOs.Lugar;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.application.Services.Implementations
{
    public class LugarService : ILugarService
    {
        private readonly IMapper _mapper;
        private readonly ILugarRepository _lugarRepository;
        private readonly IAlterarLugarNegocio _alterarLugarNegocio;
        private readonly ICriarLugarNegocio _criarLugarNegocio;
        public LugarService(ILugarRepository lugarRepository,
                            IMapper mapper,
                            IAlterarLugarNegocio alterarLugarNegocio,
                            ICriarLugarNegocio criarLugarNegocio){
            _lugarRepository = lugarRepository;
            _mapper = mapper;
            _alterarLugarNegocio = alterarLugarNegocio;
            _criarLugarNegocio = criarLugarNegocio;
        }
        public async Task<LugarDTO> AtualizarLugar(AtualizarLugarDTO lugarAtualizado,Guid IdLugar,Usuario usuarioLogado)
        {
            try{
                var lugarAtualizar = await _lugarRepository.GetLugarById(IdLugar);
                
                if(lugarAtualizar == null)
                    throw new NotFoundException("Não foi encontrado um lugar com o id especificado");
                if(usuarioLogado.Id != lugarAtualizar.UsuarioId)
                    throw new ForbiddenException("O usuário que criou o lugar é diferente do usuário logado");
                lugarAtualizar.NomeLugar = lugarAtualizado.NomeLugar;
                lugarAtualizar.Latitude = lugarAtualizado.Latitude;
                lugarAtualizar.Longitude = lugarAtualizado.Longitude;
                
                var listaErros = await _alterarLugarNegocio.Validate(lugarAtualizar,usuarioLogado.Id);
                
                if(listaErros.Count > 0)
                    throw new BusinessException<AtualizarLugarDTO>(listaErros.ToArray(),lugarAtualizado);

                await _lugarRepository.SaveChangesAsync();
                return _mapper.Map<LugarDTO>(lugarAtualizar);
                
            } 
            catch(NotFoundException e){
                throw e;
            }
            catch(ForbiddenException e){
                throw e;
            }
            catch(BusinessException<AtualizarLugarDTO> e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<LugarDTO> CriarLugar(CriarLugarDTO lugar, Usuario usuarioLogado)
        {
            try{
                var lugarCriar = _mapper.Map<Lugar>(lugar);
                
                var listaErros = await _criarLugarNegocio.Validate(lugarCriar,usuarioLogado.Id);
                
                if(listaErros.Count > 0)
                    throw new BusinessException<CriarLugarDTO>(listaErros.ToArray(),lugar);
                lugarCriar.Proprietario = usuarioLogado;
                _lugarRepository.Add(lugarCriar);
                await _lugarRepository.SaveChangesAsync();
                return _mapper.Map<LugarDTO>(lugarCriar);
                
            }
             catch(NotFoundException e){
                throw e;
            }
            catch(BusinessException<CriarLugarDTO> e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<LugarDTO> DeletarLugar(Guid idLugar,Usuario usuarioLogado)
        {
            try{
                var lugarDeletar = await _lugarRepository.GetLugarById(idLugar);
                if(lugarDeletar == null)
                    throw new NotFoundException("Não existe um lugar com o id especificado");
                
                var lugarRetorno = _mapper.Map<LugarDTO>(lugarDeletar);
                
                if(usuarioLogado.Id != lugarDeletar.UsuarioId)
                    throw new ForbiddenException("O usuário que criou o lugar é diferente do usuário logado");
                
                lugarDeletar.Deletado = true;
                
                await _lugarRepository.SaveChangesAsync();
                return _mapper.Map<LugarDTO>(lugarRetorno);
                
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(ForbiddenException e){
                throw e;
            }
            catch(BusinessException<LugarDTO> e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<LugarDTO> GetLugarById(Guid idLugar)
        {
            try{
                var lugar = await _lugarRepository.GetLugarById(idLugar);
                if(lugar == null)
                    throw new NotFoundException("Não existe um lugar com o id especificado");
                return _mapper.Map<LugarDTO>(lugar);
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<LugarPageDTO> GetLugaresPage(int pageNumber,int pageSize = 10)
        {
            try{
                var lugares = await _lugarRepository.GetLugaresPage(pageNumber);
                
                return new LugarPageDTO(){Lugares = _mapper.Map<LugarDTO[]>(lugares),PageNumber = pageNumber, PageSize = 10};
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<LugarPageDTO> GetLugaresPageByUsuario(Usuario usuarioLogado, int pageNumber,int pageSize = 10)
        {
            try{
                var lugares = await _lugarRepository.GetLugaresByUsuario(pageNumber,usuarioLogado.Id,pageSize);
                
                return new LugarPageDTO(){Lugares = _mapper.Map<LugarDTO[]>(lugares),PageNumber = pageNumber, PageSize = 10};
            }
            catch(ForbiddenException e){
                throw e;
            }catch(Exception e){
                throw e;
            }
        }
    }
}