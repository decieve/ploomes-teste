using Microsoft.EntityFrameworkCore;
using ploomes_teste.domain;
using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class LugarRepository : GeneralRepository,ILugarRepository
    {
        
        public LugarRepository(PloomesContext ploomesContext) : base(ploomesContext)
        {
        }

        public async Task<Lugar> GetLugarByCnpj(string Cnpj)
        {
            var main_query = from lugar in _context.Lugares
                            where lugar.Cnpj == Cnpj
                            select lugar;

            return await main_query
                    .Include(l => l.TipoLugar)
                    .Include(l => l.Avaliacoes).ThenInclude(a => a.Usuario)
                    .FirstOrDefaultAsync();
        }

        public async Task<Lugar> GetLugarById(Guid id)
        {
            var main_query = from lugar in _context.Lugares
                            where lugar.Id == id
                            select lugar;

            return await main_query
                        .Include(l => l.TipoLugar)
                        .Include(l => l.Avaliacoes).ThenInclude(a => a.Usuario)
                        .FirstOrDefaultAsync();
        }

        public async Task<Lugar[]> GetLugaresByUsuario(int pageNumber, string idUsuario, int pageSize = 10)
        {
            var main_query = from lugar in _context.Lugares
                            where lugar.UsuarioId == idUsuario
                            select lugar;

            return await main_query
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .Include(l => l.TipoLugar)
                .Include(l => l.Avaliacoes).ThenInclude(a => a.Usuario)
                .ToArrayAsync();
        }

        public async Task<Lugar[]> GetLugaresPage(int pageNumber, int pageSize = 10)
        {
            var main_query = from lugar in _context.Lugares
                            select lugar;

            return await main_query
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .Include(l => l.TipoLugar)
                .Include(l => l.Avaliacoes).ThenInclude(a => a.Usuario)
                .ToArrayAsync();
        }
    }
}