using Microsoft.EntityFrameworkCore;
using ploomes_teste.domain;
using ploomes_teste.domain.helpers;
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
                    .Include(l => l.Avaliacoes.Take(3)).ThenInclude(l => l.Historico)
                    .Include(l => l.Avaliacoes.Take(3)).ThenInclude(a => a.Avaliador)
                    .FirstOrDefaultAsync();
        }

        public async Task<Lugar> GetLugarById(Guid id)
        {
            var main_query = from lugar in _context.Lugares
                            where lugar.Id == id
                            select lugar;

            return await main_query
                        .Include(l => l.TipoLugar)
                        .Include(l => l.Avaliacoes.Take(3)).ThenInclude(l => l.Historico)
                        .Include(l => l.Avaliacoes.Take(3)).ThenInclude(a => a.Avaliador)
                        .FirstOrDefaultAsync();
        }

        public async Task<Lugar[]> GetLugaresByProprietario(int pageNumber, string idUsuario, int pageSize = 10)
        {
            
            var main_query = from lugar in _context.Lugares
                            where lugar.ProprietarioId == idUsuario
                            select lugar;

            return await main_query
                .Include(l => l.TipoLugar)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(l => l.Historico)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(a => a.Avaliador)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<Lugar[]> GetLugaresPage(int pageNumber, int pageSize = 10)
        {
            var main_query = from lugar in _context.Lugares
                            select lugar;

            return await main_query
                .Include(l => l.TipoLugar)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(l => l.Historico)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(a => a.Avaliador)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToArrayAsync();
        }

        public async Task<LugarDistancia[]> GetLugaresByDistanciaAvaliador(int pageNumber,double latitudeAvaliador,double longitudeAvaliador,int pageSize = 10){

             var main_query = _context.Lugares .Include(l => l.TipoLugar)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(l => l.Historico)
                .Include(l => l.Avaliacoes.Take(3)).ThenInclude(a => a.Avaliador)
                .Select(l => new LugarDistancia(){Lugar = l, Distancia = distance(l.Latitude,l.Longitude,latitudeAvaliador,longitudeAvaliador)});
                           
            return await main_query
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .ToArrayAsync();
        }


        private static double distance(double lat1, double lon1, double lat2, double lon2) {
            if ((lat1 == lat2) && (lon1 == lon2)) {
                return 0;
            }
            else {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                dist = dist * 1.609344;
    
                return (dist);
            }
        }
        private static double deg2rad(double deg) {
            return (deg * Math.PI / 180.0);
        }
        private static double rad2deg(double rad) {
            return (rad / Math.PI * 180.0);
        }

    }
   
}