using AutoMapper;
using ploomes_teste.application.DTOs.Avaliacao;
using ploomes_teste.application.Exceptions;
using ploomes_teste.application.Services.Contracts;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.application.Services.Implementations
{
    public class AvaliacaoService : IAvaliacaoService
    {   
        private readonly IMapper _mapper;
        private readonly IAvaliacaoRepository _avaliacaoRepository;
        private readonly IAlterarAvaliacaoNegocio _alterarAvaliacaoNegocio;
        private readonly ICriarAvaliacaoNegocio _criarAvaliacaoNegocio;
        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository,IMapper mapper,ICriarAvaliacaoNegocio criarAvaliacaoNegocio,IAlterarAvaliacaoNegocio alterarAvaliacaoNegocio){
            _avaliacaoRepository = avaliacaoRepository;
            _mapper = mapper;
            _criarAvaliacaoNegocio = criarAvaliacaoNegocio;
            _alterarAvaliacaoNegocio = alterarAvaliacaoNegocio;
        }
        public async Task<AvaliacaoUsuarioDTO> AtualizarAvaliacao(AtualizarAvaliacaoDTO avaliacaoAtualizada,Guid idAvaliacao,Usuario usuarioLogado)
        {
            
            var avaliacaoAtualizar = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
            if(avaliacaoAtualizar == null)
                throw new NotFoundException("Não foi encontrada uma avaliação com o id especificado");
            if(avaliacaoAtualizar.AvaliadorId != usuarioLogado.Id)
                throw new ForbiddenException("O usuário logado não é o mesmo que criou a avaliação"); 
            //Salva no histórico
            HistoricoAvaliacao historico = new();
            historico.Anonimo = avaliacaoAtualizar.Anonimo;
            historico.Descricao = avaliacaoAtualizar.Descricao;
            historico.NotaAmbiente = avaliacaoAtualizar.NotaAmbiente;
            historico.NotaAtendimento = avaliacaoAtualizar.NotaAtendimento;
            historico.NotaPreco = avaliacaoAtualizar.NotaPreco;
            historico.NotaQualidade = avaliacaoAtualizar.NotaQualidade;
            historico.DataModificacao = DateTime.Now;
            
            // Atualiza a avaliação
            avaliacaoAtualizar.NotaPreco = avaliacaoAtualizada.NotaPreco;
            avaliacaoAtualizar.NotaAmbiente = avaliacaoAtualizada.NotaAmbiente;
            avaliacaoAtualizar.NotaQualidade = avaliacaoAtualizada.NotaQualidade;
            avaliacaoAtualizar.NotaAtendimento = avaliacaoAtualizada.NotaAtendimento;
            avaliacaoAtualizar.Descricao = avaliacaoAtualizada.Descricao;
            avaliacaoAtualizar.Anonimo = avaliacaoAtualizada.Anonimo;
            avaliacaoAtualizar.DataAtualizada = DateTime.Now;
            historico.AvaliacaoAtual =avaliacaoAtualizar;
            
            // valida a avaliação atualizada
            var listaErros = await _alterarAvaliacaoNegocio.Validate(_mapper.Map<Avaliacao>(avaliacaoAtualizar),usuarioLogado.Id);
            
            if(listaErros.Count > 0)
                throw new BusinessException<AtualizarAvaliacaoDTO>(listaErros.ToArray(),avaliacaoAtualizada);
            
            
            _avaliacaoRepository.Add(historico);
            await _avaliacaoRepository.SaveChangesAsync();
            return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoAtualizar);
        }

        public async Task<AvaliacaoUsuarioDTO> CriarAvaliacao(CriarAvaliacaoDTO avaliacao,Usuario usuarioLogado)
        {
            
            var avaliacaoCriar = _mapper.Map<Avaliacao>(avaliacao);

            var listaErros = await _criarAvaliacaoNegocio.Validate(avaliacaoCriar,usuarioLogado.Id);
            
            if(listaErros.Count > 0)
                throw new BusinessException<CriarAvaliacaoDTO>(listaErros.ToArray(),avaliacao);
            avaliacaoCriar.Avaliador = usuarioLogado;
            avaliacaoCriar.DataAtualizada = DateTime.Now;
            avaliacaoCriar.DataPostada = DateTime.Now;
            _avaliacaoRepository.Add(avaliacaoCriar);
            await _avaliacaoRepository.SaveChangesAsync();
            return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoCriar);
                
           
        }

        public async Task<AvaliacaoUsuarioDTO> DeletarAvaliacao(Guid idAvaliacao,Usuario usuarioLogado)
        {

            var avaliacaoDeletar = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
            if(avaliacaoDeletar == null)
                throw new NotFoundException("Não existe um avaliacao com o id especificado");
            
            var avaliacaoRetorno = _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoDeletar);
            
            if(avaliacaoDeletar.AvaliadorId != usuarioLogado.Id)
                throw new ForbiddenException("O usuário logado não é o mesmo que criou a avaliação"); 
            
            avaliacaoDeletar.Deletado = true;
            if(avaliacaoDeletar.Historico != null){
                foreach(HistoricoAvaliacao h in avaliacaoDeletar.Historico ){
                    h.Deletado = true;
                }
            }
                
            await _avaliacaoRepository.SaveChangesAsync();
            return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoRetorno);
          
        
        }

        public async Task<AvaliacaoUsuarioDTO> GetAvaliacaoById(Guid idAvaliacao, bool includeHistorico = true)
        {
            
            var avaliacao = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
            if(avaliacao == null)
                throw new NotFoundException("Não existe um avaliacao com o id especificado");
            return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacao);
        }

        public async Task<AvaliacaoLugarPageDTO> GetAvaliacoesPageByLugar(Guid idLugar, int pageNumber,int pageSize = 10, bool includeHistorico = false)
        {
            if(pageNumber < 1)
                throw new InvalidPageException("O número da página deve ser maior que 0");
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByLugarPage(pageNumber,idLugar,pageSize,includeHistorico);

            return new AvaliacaoLugarPageDTO(){Avaliacoes = _mapper.Map<AvaliacaoLugarDTO[]>(avaliacoes),PageNumber = pageNumber, PageSize = 10};

        }

        public async Task<AvaliacaoUsuarioPageDTO> GetAvaliacoesPageByAvaliador(Usuario usuario, int pageNumber,int pageSize = 10, bool includeHistorico = false)
        {
            if(pageNumber < 1)
                throw new InvalidPageException("O número da página deve ser maior que 0");
            
            var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByAvaliador(pageNumber,usuario.Id,pageSize,includeHistorico);

            return new AvaliacaoUsuarioPageDTO(){Avaliacoes = _mapper.Map<AvaliacaoUsuarioDTO[]>(avaliacoes),PageNumber = pageNumber, PageSize = 10};

        }
    }
}