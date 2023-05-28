using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class CriarAvaliacaoNegocio : ICriarAvaliacaoNegocio
    {

        private IAvaliacaoRepository _avaliacaoRepository {get;set;}
        private ILugarRepository _lugarRepository {get;set;}
        public CriarAvaliacaoNegocio(IAvaliacaoRepository avaliacaoRepository, ILugarRepository lugarRepository){
            _avaliacaoRepository = avaliacaoRepository;
            _lugarRepository = lugarRepository;
        }
        public async Task<bool> ValidateAvaliacaoDuplicada(Guid LugarAvaliacaoId,string AvaliadorId)
        {
            try{
                var av = await _avaliacaoRepository.GetAvaliacaoByIdLugarIdAvaliador(LugarAvaliacaoId,AvaliadorId);
                return av == null;
            }catch(Exception e){
                throw e;
            }
        }
        public async Task<bool> ValidateLugarExiste(Guid LugarAvaliacaoId){
            try{
                var l = await _lugarRepository.GetLugarById(LugarAvaliacaoId);
                return l != null;
            }catch(Exception e){
                throw e;
            }
        }
        public bool ValidateNota(double nota)
        {
            return nota >= 0.0 && nota<= 5.0; 
        }

      
        public async Task<List<string>> Validate(Avaliacao avaliacao,string idUsuarioLogado)
        {
            List<string> validationResult =new();

            if (!ValidateNota(avaliacao.NotaAmbiente))
                validationResult.Add("A nota do ambiente não está entre 0.0 e 5.0");
            
            if (!ValidateNota(avaliacao.NotaAtendimento))
                validationResult.Add("A nota do atendimento não está entre 0.0 e 5.0");

            if (!ValidateNota(avaliacao.NotaPreco))
                validationResult.Add("A nota do preço não está entre 0.0 e 5.0");

            if (!ValidateNota(avaliacao.NotaQualidade))
                validationResult.Add("A nota da qualidade não está entre 0.0 e 5.0");

            if (! await ValidateAvaliacaoDuplicada(avaliacao.LugarId,idUsuarioLogado))
                validationResult.Add("O usuário já avaliou o lugar");
            if (! await ValidateLugarExiste(avaliacao.LugarId))
                validationResult.Add("O lugar especificado não existe");
            return validationResult;
        }
    }
}