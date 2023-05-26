using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ploomes_teste.domain;

namespace ploomes_teste.persistence.Contexts
{
    public class PloomesContext : IdentityDbContext<Usuario>
    {
        public PloomesContext(DbContextOptions<PloomesContext> options)
                    : base(options)
        {
        }

        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<HistoricoAvaliacao> HistoricosAvaliacao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Avaliacao>().HasQueryFilter(a => !a.Deletado);
            modelBuilder.Entity<HistoricoAvaliacao>().HasQueryFilter(h => !h.Deletado);
            modelBuilder.Entity<Lugar>().HasQueryFilter(l => !l.Deletado);
        }
    }
}