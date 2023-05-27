using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;
using ploomes_teste.negocio.Contracts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.negocio.Implementations
{
    public class AlterarAvaliacaoNegocio : IAlterarAvaliacaoNegocio
    {   
        private Usuario _usuarioLogado {get;set;}

        private Avaliacao _avaliacao {get;set;}
        private IAvaliacaoRepository _avaliacaoRepository {get;set;}
        private ILugarRepository _lugarRepository {get;set;}
        public AlterarAvaliacaoNegocio(Usuario usuario,Avaliacao avaliacao,IAvaliacaoRepository avaliacaoRepository, ILugarRepository lugarRepository){
            _usuarioLogado = usuario;
            _avaliacao = avaliacao;
            _avaliacaoRepository = avaliacaoRepository;
            _lugarRepository = lugarRepository;
        }

        public bool ValidateNotaAmbiente(double notaAmbiente)
        {
            return notaAmbiente >= 0.0 && notaAmbiente <= 5.0; 
        }

        public bool ValidateNotaAtendimento(double notaAtendimento)
        {
            return notaAtendimento >= 0.0 && notaAtendimento <= 5.0; 
        }

        public bool ValidateNotaPreco(double notaPreco)
        {
            return notaPreco >= 0.0 && notaPreco <= 5.0; 
        }

        public bool ValidateNotaQualidade(double notaQualidade)
        {
            return notaQualidade >= 0.0 && notaQualidade <= 5.0; 
        }

        public async Task<List<string>> Validate(Avaliacao avaliacao,string idUsuarioLogado)
        {
            List<string> validationResult =new();

          
            if (!ValidateNotaAmbiente(avaliacao.NotaAmbiente))
                validationResult.Add("A nota do ambiente não está entre 0.0 e 5.0");
            
            if (!ValidateNotaAmbiente(avaliacao.NotaAtendimento))
                validationResult.Add("A nota do atendimento não está entre 0.0 e 5.0");

            if (!ValidateNotaAmbiente(avaliacao.NotaPreco))
                validationResult.Add("A nota do preço não está entre 0.0 e 5.0");

            if (!ValidateNotaAmbiente(avaliacao.NotaQualidade))
                validationResult.Add("A nota da qualidade não está entre 0.0 e 5.0");

            return validationResult;
        }
    }
}