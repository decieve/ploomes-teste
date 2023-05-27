using System.ComponentModel.DataAnnotations;
using ploomes_teste.domain;

namespace ploomes_teste.negocio.Contracts
{
    public interface ICriarAvaliacaoNegocio 
    {
        bool ValidateNotaAmbiente(double notaAmbiente);
        bool ValidateNotaPreco(double notaPreco);
        bool ValidateNotaQualidade(double notaQualidade);
        bool ValidateNotaAtendimento(double notaAtendimento);
        Task<bool> ValidateAvaliacaoDuplicada(Guid LugarAvaliacaoId,string UsuarioAvaliacaoId);
        Task<List<string>> Validate(Avaliacao avaliacao,string idUsuarioLogado);
        Task<bool> ValidateLugarExiste(Guid LugarAvaliacaoId);
    }
}