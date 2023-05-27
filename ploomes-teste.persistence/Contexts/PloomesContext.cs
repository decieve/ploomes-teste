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
        public DbSet<TipoLugar> TiposLugar{get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lugar>().HasIndex(e => e.Cnpj).IsUnique();
            
            modelBuilder.Entity<Lugar>().HasOne(e => e.Proprietario).WithMany(l => l.Lugares).HasForeignKey(e => e.UsuarioId);
            
            
            
            modelBuilder.Entity<Avaliacao>().HasOne(e => e.Lugar).WithMany(l => l.Avaliacoes).HasForeignKey(e => e.LugarId);
            
            modelBuilder.Entity<Avaliacao>().HasOne(e => e.Avaliador).WithMany(l => l.Avaliacoes).HasForeignKey(e => e.UsuarioId);

            modelBuilder.Entity<Avaliacao>().HasIndex(e => new {e.LugarId, e.UsuarioId}).IsUnique();



            modelBuilder.Entity<Avaliacao>().HasQueryFilter(a => !a.Deletado);
            
            modelBuilder.Entity<HistoricoAvaliacao>().HasQueryFilter(h => !h.Deletado);
            
            modelBuilder.Entity<Lugar>().HasQueryFilter(l => !l.Deletado);
        }
    }
}