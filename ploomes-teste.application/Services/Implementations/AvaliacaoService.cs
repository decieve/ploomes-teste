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
        public AvaliacaoService(IAvaliacaoRepository avaliacaoRepository,IMapper mapper){
            _avaliacaoRepository = avaliacaoRepository;
            _mapper = mapper;
        }
        public async Task<AvaliacaoUsuarioDTO> AtualizarAvaliacao(CriarAtualizarAvaliacaoDTO avaliacaoAtualizada,Guid idAvaliacao,Usuario usuarioLogado)
        {
            try{
                var avaliacaoAtualizar = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
                if(avaliacaoAtualizar == null)
                    throw new NotFoundException("Não foi encontrada uma avaliação com o id especificado");
                if(avaliacaoAtualizar.UsuarioId != usuarioLogado.Id)
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
                avaliacaoAtualizar.Historico.Add(historico);
                
                // valida a avaliação atualizada
                var listaErros = await _alterarAvaliacaoNegocio.Validate(_mapper.Map<Avaliacao>(avaliacaoAtualizar),usuarioLogado.Id);
                
                if(listaErros.Count > 0)
                    throw new BusinessException<CriarAtualizarAvaliacaoDTO>(listaErros.ToArray(),avaliacaoAtualizada);
                
                
                await _avaliacaoRepository.SaveChangesAsync();
                return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoAtualizar);
                
            } 
            catch(NotFoundException e){
                throw e;
            }
            catch(BusinessException<AvaliacaoUsuarioDTO> e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<AvaliacaoUsuarioDTO> CriarAvaliacao(CriarAtualizarAvaliacaoDTO avaliacao,Usuario usuarioLogado)
        {
            try{
                var avaliacaoCriar = _mapper.Map<Avaliacao>(avaliacao);
                
                var listaErros = await _criarAvaliacaoNegocio.Validate(avaliacaoCriar,usuarioLogado.Id);
                
                if(listaErros.Count > 0)
                    throw new BusinessException<CriarAtualizarAvaliacaoDTO>(listaErros.ToArray(),avaliacao);
                avaliacaoCriar.Avaliador = usuarioLogado;
                _avaliacaoRepository.Add(avaliacaoCriar);
                await _avaliacaoRepository.SaveChangesAsync();
                return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoCriar);
                
            } 
            catch(NotFoundException e){
                throw e;
            }
            catch(BusinessException<AvaliacaoUsuarioDTO> e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<AvaliacaoUsuarioDTO> DeletarAvaliacao(Guid idAvaliacao,Usuario usuarioLogado)
        {
            try{
                var avaliacaoDeletar = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
                if(avaliacaoDeletar == null)
                    throw new NotFoundException("Não existe um avaliacao com o id especificado");
                
                var avaliacaoRetorno = _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoDeletar);
                
                if(avaliacaoDeletar.UsuarioId != usuarioLogado.Id)
                    throw new ForbiddenException("O usuário logado não é o mesmo que criou a avaliação"); 
                
                avaliacaoDeletar.Deletado = true;
                
                await _avaliacaoRepository.SaveChangesAsync();
                return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacaoRetorno);
                
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(BusinessException<AvaliacaoUsuarioDTO> e){
                throw e;
            } 
            catch(Exception e){
                throw e;
            }
        }

        public async Task<AvaliacaoUsuarioDTO> GetAvaliacaoById(Guid idAvaliacao, bool includeHistorico = true)
        {
            try{
                var avaliacao = await _avaliacaoRepository.GetAvaliacaoById(idAvaliacao);
                if(avaliacao == null)
                    throw new NotFoundException("Não existe um avaliacao com o id especificado");
                return _mapper.Map<AvaliacaoUsuarioDTO>(avaliacao);
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<AvaliacaoLugarPageDTO> GetAvaliacoesPageByLugar(Guid idLugar, int pageNumber = 0,int pageSize = 10, bool includeHistorico = false)
        {
            try{
                var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByLugarPage(pageNumber,idLugar,pageSize,includeHistorico);

                return new AvaliacaoLugarPageDTO(){Avaliacoes = _mapper.Map<AvaliacaoLugarDTO[]>(avaliacoes),PageNumber = pageNumber, PageSize = 10};
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }

        public async Task<AvaliacaoUsuarioPageDTO> GetAvaliacoesPageByUsuario(Usuario usuario, int pageNumber = 0,int pageSize = 10, bool includeHistorico = false)
        {
            try{
                var avaliacoes = await _avaliacaoRepository.GetAvaliacoesByUsuario(pageNumber,usuario.Id,pageSize,includeHistorico);

                return new AvaliacaoUsuarioPageDTO(){Avaliacoes = _mapper.Map<AvaliacaoUsuarioDTO[]>(avaliacoes),PageNumber = pageNumber, PageSize = 10};
            }
            catch(NotFoundException e){
                throw e;
            }
            catch(Exception e){
                throw e;
            }
        }
    }
}