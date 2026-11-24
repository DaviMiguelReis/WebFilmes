
using WebFilmes.Data;
using WebFilmes.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFilmes.Repositories
{
    public class FilmeRepository(FilmesContext context) : IFilmeRepository
    {
        public async Task<IReadOnlyList<Filme>> ListarAsync(string? termoBusca, CancellationToken cancellationToken = default)
        {
            var consulta = context.Filmes.AsNoTracking();

            if(!string.IsNullOrWhiteSpace(termoBusca))
            {
                var termo = termoBusca.Trim(); //tratando valor
                consulta = consulta.Where(filme => filme.Titulo.Contains(termo) || filme.Genero.Contains(termo));

            }
            return await consulta.OrderBy(filme=> filme.Titulo).ToListAsync(cancellationToken);
        }

        public async Task<Filme?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default) =>
            await context.Filmes.FirstOrDefaultAsync(filme => filme.Id == id,cancellationToken);
        
        public async Task<Filme?> ObterPorIdAParaLeiturAsync(int id, CancellationToken cancellationToken = default) =>
            await context.Filmes.FirstOrDefaultAsync(filme => filme.Id == id, cancellationToken);
        
        public async Task<bool> ExisteAlgumAsync(int id, CancellationToken cancellationToken = default) =>
             await context.Filmes.AnyAsync(cancellationToken);
        
        public async Task AdicionarAsync(Filme filme, CancellationToken cancellationToken) =>
            await context.Filmes.AddAsync(filme, cancellationToken);
        
        public async void Remover(Filme filme ) => context.Filmes.Remove(filme);

        public async Task SalvarAsync(CancellationToken cancellationToken) =>
              await context.SaveChangesAsync(cancellationToken);

    }
}
