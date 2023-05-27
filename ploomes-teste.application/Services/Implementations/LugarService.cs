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
        private readonly ITipoLugarRepository _tipoLugarRepository;
        private readonly IAlterarLugarNegocio _alterarLugarNegocio;
        private readonly ICriarLugarNegocio _criarLugarNegocio;
        public LugarService(ILugarRepository lugarRepository,
                            IMapper mapper,
                            IAlterarLugarNegocio alterarLugarNegocio,
                            ICriarLugarNegocio criarLugarNegocio,
                            ITipoLugarRepository tipoLugarRepository){
            _lugarRepository = lugarRepository;
            _mapper = mapper;
            _alterarLugarNegocio = alterarLugarNegocio;
            _criarLugarNegocio = criarLugarNegocio;
            _tipoLugarRepository = tipoLugarRepository;
        }
        public async Task<LugarDTO> AtualizarLugar(AtualizarLugarDTO lugarAtualizado,Guid IdLugar,Usuario usuarioLogado)
        {
          
            var lugarAtualizar = await _lugarRepository.GetLugarById(IdLugar);
            
            if(lugarAtualizar == null)
                throw new NotFoundException("Não foi encontrado um lugar com o id especificado");
            if(usuarioLogado.Id != lugarAtualizar.ProprietarioId)
                throw new ForbiddenException("O usuário que criou o lugar é diferente do usuário logado");
            lugarAtualizar.NomeLugar = lugarAtualizado.NomeLugar;
            lugarAtualizar.Latitude = lugarAtualizado.Latitude.Value;
            lugarAtualizar.Longitude = lugarAtualizado.Longitude.Value;
            
            var listaErros = await _alterarLugarNegocio.Validate(lugarAtualizar,usuarioLogado.Id);
            
            if(listaErros.Count > 0)
                throw new BusinessException<AtualizarLugarDTO>(listaErros.ToArray(),lugarAtualizado);

            await _lugarRepository.SaveChangesAsync();
            return _mapper.Map<LugarDTO>(lugarAtualizar);
            
        }

        public async Task<LugarDTO> CriarLugar(CriarLugarDTO lugar, Usuario usuarioLogado)
        {
            
            var lugarCriar = _mapper.Map<Lugar>(lugar);
            
            var listaErros = await _criarLugarNegocio.Validate(lugarCriar,usuarioLogado.Id);
            
            if(listaErros.Count > 0)
                throw new BusinessException<CriarLugarDTO>(listaErros.ToArray(),lugar);
            lugarCriar.Proprietario = usuarioLogado;
            _lugarRepository.Add(lugarCriar);
            await _lugarRepository.SaveChangesAsync();
            return _mapper.Map<LugarDTO>(lugarCriar);

        }

        public async Task<LugarDTO> DeletarLugar(Guid idLugar,Usuario usuarioLogado)
        {
            
            var lugarDeletar = await _lugarRepository.GetLugarById(idLugar);
            if(lugarDeletar == null)
                throw new NotFoundException("Não existe um lugar com o id especificado");
            
            var lugarRetorno = _mapper.Map<LugarDTO>(lugarDeletar);
            
            if(usuarioLogado.Id != lugarDeletar.ProprietarioId)
                throw new ForbiddenException("O usuário que criou o lugar é diferente do usuário logado");
            
            lugarDeletar.Deletado = true;
            foreach(Avaliacao a in lugarDeletar.Avaliacoes){
                a.Deletado = true;
            }
            await _lugarRepository.SaveChangesAsync();
            return _mapper.Map<LugarDTO>(lugarRetorno);

        }

        public async Task<LugarDTO> GetLugarById(Guid idLugar)
        {
            
            var lugar = await _lugarRepository.GetLugarById(idLugar);
            if(lugar == null)
                throw new NotFoundException("Não existe um lugar com o id especificado");
            return _mapper.Map<LugarDTO>(lugar);
           
        }

        public async Task<LugarPageDTO> GetLugaresPage(int pageNumber,int pageSize = 10)
        {
            
            var lugares = await _lugarRepository.GetLugaresPage(pageNumber);
            
            return new LugarPageDTO(){Lugares = _mapper.Map<LugarDTO[]>(lugares),PageNumber = pageNumber, PageSize = 10};
            
        }

        public async Task<LugarPageDTO> GetLugaresPageByProprietario(Usuario usuarioLogado, int pageNumber,int pageSize = 10)
        {
            
            var lugares = await _lugarRepository.GetLugaresByProprietario(pageNumber,usuarioLogado.Id,pageSize);
            
            return new LugarPageDTO(){Lugares = _mapper.Map<LugarDTO[]>(lugares),PageNumber = pageNumber, PageSize = 10};
            
        }
        public async Task<LugarPageDTO> GetLugaresPageByLocalizacaoAvaliador(Usuario usuarioLogado, int pageNumber,int pageSize = 10)
        {
            var lugares = await _lugarRepository.GetLugaresByDistanciaAvaliador(pageNumber,usuarioLogado.LatitudeMoradia.Value,usuarioLogado.LongitudeMoradia.Value,pageSize);
            
            return new LugarPageDTO(){Lugares = _mapper.Map<LugarDTO[]>(lugares),PageNumber = pageNumber, PageSize = 10};
        }
        public async Task<TipoLugarDTO[]> GetTiposLugares(){
           
            var tiposLugares = await _tipoLugarRepository.GetTiposLugar();
            return _mapper.Map<TipoLugarDTO[]>(tiposLugares);
           
        }
    }
}