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
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Lugar>().HasOne(e => e.Proprietario).WithMany(l => l.Lugares).HasForeignKey(e => e.ProprietarioId).OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<TipoLugar>().HasData(Enum.GetValues(typeof(TipoLugarEnum))
                .Cast<TipoLugarEnum>()
                .Select(e => new TipoLugar
                {
                    Id = (short)e,
                    Nome = e.ToString()
                }));
            
            modelBuilder.Entity<Lugar>().HasOne(e => e.TipoLugar).WithMany(t => t.Lugares).HasForeignKey(l => l.TipoLugarId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Avaliacao>().HasOne(e => e.Lugar).WithMany(l => l.Avaliacoes).HasForeignKey(e => e.LugarId).OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Avaliacao>().HasOne(e => e.Avaliador).WithMany(l => l.Avaliacoes).HasForeignKey(e => e.AvaliadorId).OnDelete(DeleteBehavior.Restrict);

            



            modelBuilder.Entity<Avaliacao>().HasQueryFilter(a => !a.Deletado);
            
            modelBuilder.Entity<HistoricoAvaliacao>().HasQueryFilter(h => !h.Deletado);
            
            modelBuilder.Entity<Lugar>().HasQueryFilter(l => !l.Deletado);
        }
    }
}