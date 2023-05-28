using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;

namespace ploomes_teste.negocio.Contracts
{
    public interface IAlterarAvaliacaoNegocio 
    {
        bool ValidateNota(double nota);

        Task<List<string>> Validate(Avaliacao avaliacao,string idUsuarioLogado);


    }
}