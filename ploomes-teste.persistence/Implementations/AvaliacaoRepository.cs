using Microsoft.EntityFrameworkCore;
using ploomes_teste.domain;
using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class AvaliacaoRepository : GeneralRepository,IAvaliacaoRepository
    {
        public AvaliacaoRepository(PloomesContext ploomesContext) : base(ploomesContext)
        {
        }

        public async Task<Avaliacao> GetAvaliacaoById(Guid id, bool includeHistorico = true)
        {
            var main_query = from avaliacao in _context.Avaliacoes
                            where avaliacao.Id == id
                            select avaliacao;
            if(includeHistorico)
                main_query.Include(a => a.Historico);

            return await main_query
                        .Include(a => a.Avaliador)
                        .FirstOrDefaultAsync();
        }

        public async Task<Avaliacao[]> GetAvaliacoesByAvaliador(int pageNumber, string idAvaliador, int pageSize = 10, bool includeHistorico = false)
        {
            var main_query = from avaliacao in _context.Avaliacoes
                            where avaliacao.Avaliador.Id == idAvaliador
                            orderby avaliacao.DataAtualizada descending
                            select avaliacao;

            if(includeHistorico)
                main_query.Include(a => a.Historico);
                
            return await main_query
                .Include(a => a.Avaliador)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Avaliacao[]> GetAvaliacoesByLugarPage(int pageNumber,Guid idLugar, int pageSize = 10, bool includeHistorico = false)
        {
             var main_query = from avaliacao in _context.Avaliacoes
                            where avaliacao.Lugar.Id == idLugar
                            orderby avaliacao.DataAtualizada descending
                            select avaliacao;
                            
            if(includeHistorico)
                main_query.Include(a => a.Historico);
                
            return await main_query
                .Include(a => a.Avaliador)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Avaliacao> GetAvaliacaoByIdLugarIdAvaliador(Guid idLugar, string idAvaliador, bool includeHistorico = false)
        {
               var main_query = from avaliacao in _context.Avaliacoes
                            where avaliacao.LugarId == idLugar && avaliacao.AvaliadorId == idAvaliador
                            select avaliacao;
            if(includeHistorico)
                main_query.Include(a => a.Historico);

            return await main_query
                        .Include(a => a.Avaliador)
                        .FirstOrDefaultAsync();
        }
    }
}