using WebFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFilmes.Data
{
    public class FilmesContext(DbContextOptions<FilmesContext> options) : DbContext(options)
    {
        public DbSet<Filme> Filmes => Set<Filme>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Filme>(filme =>
            {

                filme.Property(f => f.Titulo).IsRequired().HasMaxLength(150);

                filme.Property(f => f.Genero).IsRequired().HasMaxLength(80);

                filme.Property(f => f.Sinopse).IsRequired().HasMaxLength(1000);

                filme.Property(f => f.Nota).IsRequired().HasPrecision(3, 1);

                filme.Property(f => f.AnoLancamento).IsRequired();

                filme.Property(f => f.ImagemUrl).IsRequired().HasMaxLength(500);

                filme.HasIndex(f => f.Titulo);

            });
        }

    }
}
