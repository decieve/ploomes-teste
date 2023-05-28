using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class AlterarAvaliacaoNegocio : IAlterarAvaliacaoNegocio
    {   

        private IAvaliacaoRepository _avaliacaoRepository {get;set;}
        private ILugarRepository _lugarRepository {get;set;}
        public AlterarAvaliacaoNegocio(IAvaliacaoRepository avaliacaoRepository, ILugarRepository lugarRepository){

            _avaliacaoRepository = avaliacaoRepository;
            _lugarRepository = lugarRepository;
        }

        public bool ValidateNota(double nota)
        {
            return nota >= 0.0 && nota <= 5.0; 
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

            return validationResult;
        }
    }
}