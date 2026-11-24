using WebFilmes.Models;
using WebFilmes.Data;
using Microsoft.EntityFrameworkCore;
namespace WebFilmes.Repositories
{
    public interface IFilmeRepository
    {
        Task<IReadOnlyList<Filme>> ListarAsync(string? termoBusca, CancellationToken cancellationToken = default);
        Task<Filme?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Filme?> ObterPorIdAParaLeiturAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExisteAlgumAsync(int id, CancellationToken cancellationToken = default);
        Task AdicionarAsync(Filme filme, CancellationToken cancellationToken = default);
        void Remover(Filme filme);
        Task SalvarAsync(CancellationToken cancellationToken = default);
    }
}
