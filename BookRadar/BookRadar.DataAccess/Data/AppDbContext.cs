using BookRadar.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookRadar.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<HistorialBusqueda> HistorialBusquedas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CfgHistorialBusquedas(modelBuilder);
        }

        private void CfgHistorialBusquedas(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistorialBusqueda>(entity =>
            {
                entity.ToTable("HistorialBusquedas");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Autor)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(300);
                entity.Property(e => e.Editorial)
                    .HasMaxLength(300);
                entity.Property(e => e.FechaConsulta)
                    .HasDefaultValueSql("SYSUTCDATETIME()");
                entity.HasIndex(e => new { e.Autor, e.FechaConsulta });

            });
        }
    }
}