using Microsoft.EntityFrameworkCore;
using WebFilmes.Data;
using WebFilmes.Models;
using WebFilmes.Repositories;
using WebFilmes.ViewModels;
using static System.Net.WebRequestMethods;

namespace WebFilmes.Services
{
    public class FilmeService(IFilmeRepository repositorio, ImagemService imagemService)
    {
        public async Task<FilmeIndexViewModel> ListarAsync(string? termoBusca, CancellationToken cancellationToken = default)
        {
            var filmes = await repositorio.ListarAsync(termoBusca, cancellationToken);

            return new FilmeIndexViewModel
            {
                TermoBusca = termoBusca,
                Filmes = filmes.Select(ToViewModel).ToList()
            };
        }

        public async Task<FilmeViewModel?> ObterAsync(int id, CancellationToken cancellationToken = default)
        {
            var filme = await repositorio.ObterPorIdAParaLeiturAsync(id, cancellationToken);
            return filme is null ? null : ToViewModel(filme);
        }

        public async Task<FilmeFormViewModel?> ObterParaEdicaoAsync(int id, CancellationToken cancellationToken = default)
        {
            var filme = await repositorio.ObterPorIdAParaLeiturAsync(id, cancellationToken);

            return filme is null 
                ? null 
                : new FilmeFormViewModel
            {
                Id = filme.Id,
                Titulo = filme.Titulo,
                Genero = filme.Genero,
                AnoLancamento = filme.AnoLancamento,
                Nota = filme.Nota,
                Sinopse = filme.Sinopse,
                ImagemUrl = filme.ImagemUrl,
            };

            
        }

        public async Task PreencherImagemAtualAsync(FilmeFormViewModel modelo, CancellationToken cancellationToken = default)
        {
            var filme = await repositorio.ObterPorIdAParaLeiturAsync(modelo.Id, cancellationToken);
            modelo.ImagemUrl = filme?.ImagemUrl;
        }



        public async Task<ResultadoServico> CriarAsync (FilmeFormViewModel modelo,CancellationToken cancellationToken = default)
        {
            var (extensao, erro) = await imagemService.ValidarAsync(modelo.Imagem, cancellationToken);
            if(erro is not null)
            {
                return ResultadoServico.ImagemInvalida(erro);
            }

            var filme = new Filme
            {
                Titulo = modelo.Titulo.Trim(),
                Genero = modelo.Genero.Trim(),
                AnoLancamento = modelo.AnoLancamento,
                Nota = modelo.Nota,
                Sinopse = modelo.Sinopse!
                
            };

            string? imagemSalva =null;
            if (extensao is not null)
            {
                imagemSalva = await imagemService.SalvarAsync(modelo.Imagem!, extensao, cancellationToken);
                filme.ImagemUrl = imagemSalva;
            }

            try
            {
                await repositorio.AdicionarAsync(filme, cancellationToken);
                await repositorio.SalvarAsync(cancellationToken);
            }
            catch
            {
                imagemService.Excluir(imagemSalva);
                throw;
            }

            return ResultadoServico.Ok();

        }

        public async Task<ResultadoServico> EditarAsync(FilmeFormViewModel modelo, CancellationToken cancellationToken = default)
        {
            var filme = await repositorio.ObterPorIdAsync(modelo.Id, cancellationToken);
            if(filme is null)
            {
                return ResultadoServico.FilmeNaoEncontrado();
            }

            var (extensao, erro) = await imagemService.ValidarAsync(modelo.Imagem, cancellationToken);
            if(erro is not null)
            {
                modelo.ImagemUrl = filme.ImagemUrl;
                return ResultadoServico.ImagemInvalida(erro);
            }

            var imagemAntiga = filme.ImagemUrl;

            filme.Titulo = modelo.Titulo.Trim();
            filme.Genero = modelo.Genero.Trim();
            filme.AnoLancamento = modelo.AnoLancamento;
            filme.Nota = modelo.Nota;
            filme.Sinopse = modelo.Sinopse!;

            string? imagemNova = null;
            if(extensao is not null )
            {
                imagemNova = await imagemService.SalvarAsync(modelo.Imagem!, extensao, cancellationToken);
                filme.ImagemUrl = imagemNova;
            }
            try
            {
                await repositorio.SalvarAsync(cancellationToken);

            }
            catch
            {
                imagemService.Excluir(imagemNova);
                throw;
            }

            if(imagemNova is not null )
            {
                imagemService.Excluir(imagemAntiga);
            }
            return ResultadoServico.Ok();

        }

        public async Task ExcluirAsync (int id, CancellationToken cancellationToken = default)
        {
            var filme = await repositorio.ObterPorIdAsync(id, cancellationToken);
            if(filme is null)
            {
                return;
            }

            repositorio.Remover(filme);
            await repositorio.SalvarAsync(cancellationToken);
            imagemService.Excluir(filme.ImagemUrl);
        }

        private static FilmeViewModel ToViewModel(Filme filme) => new()
        {
            Id = filme.Id,
            Titulo = filme.Titulo,
            Genero = filme.Genero,
            AnoLancamento = filme.AnoLancamento,
            Nota = filme.Nota,
            Sinopse = filme.Sinopse,
            ImagemUrl = filme.ImagemUrl
        };

    }
}
