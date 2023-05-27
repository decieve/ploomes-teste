using Microsoft.EntityFrameworkCore;
using ploomes_teste.persistence.Contexts;
using ploomes_teste.persistence.Contracts;

namespace ploomes_teste.persistence.Implementations
{
    public class TipoLugarRepository : GeneralRepository, ITipoLugarRepository
    {
        public TipoLugarRepository(PloomesContext ploomesContext) : base(ploomesContext)
        {
        }
        public async Task<TipoLugar> GetTipoLugarById(int id)
        {
            var main_query = from tipoLugar in _context.TiposLugar
                            where tipoLugar.Id == id
                            select tipoLugar;

            return await main_query.FirstOrDefaultAsync();
        }
        public async Task<TipoLugar[]> GetTiposLugar()
        {
            var main_query = from tipoLugar in _context.TiposLugar
                            select tipoLugar;

            return await main_query.ToArrayAsync();
        }
    }
}