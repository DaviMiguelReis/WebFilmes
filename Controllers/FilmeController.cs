
using Microsoft.AspNetCore.Mvc;
using WebFilmes.Repositories;
using WebFilmes.Services;
using WebFilmes.ViewModels;

namespace WebFilmes.Controllers
{
    public class FilmeController(FilmeService filmeService, IFilmeRepository repositorio) : Controller
    {
        public async Task<IActionResult> Index(string? termoBusca,CancellationToken cancellationToken = default)
           /* => View(await repositorio.ListarAsync(termoBusca, cancellationToken))*/
        {
            var filmes = await filmeService.ListarAsync(
                termoBusca, cancellationToken);

            return View(filmes);
        }

        // CADASTRAR - GET
        [HttpGet]
        public IActionResult Criar()
        {
            return View(new FilmeFormViewModel
            {
                AnoLancamento = DateTime.Today.Year
            });
        }

        // CADASTRAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(
            FilmeFormViewModel modelo,
            CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await filmeService.CriarAsync(
                modelo, cancellationToken);

            if (resultado.ErroImagem is not null)
            {
                ModelState.AddModelError(
                    nameof(modelo.Imagem),
                    resultado.ErroImagem);

                return View(modelo);
            }

            return RedirectToAction(nameof(Index));
        }

        // DETALHES
        [HttpGet]
        public async Task<IActionResult> Detalhes(
            int? id,
            CancellationToken cancellationToken = default)
        {
            if (id is null)
                return NotFound();

            var filme = await filmeService.ObterAsync(id.Value, cancellationToken);

            if (filme is null)return NotFound();

            return View(filme);
        }

        // EDITAR - GET
        [HttpGet]
        public async Task<IActionResult> Editar(
            int? id,
            CancellationToken cancellationToken = default)
        {
            if (id is null)
                return NotFound();

            var filme = await filmeService.ObterParaEdicaoAsync(
                id.Value, cancellationToken);

            if (filme is null)
                return NotFound();

            return View(filme);
        }

        // EDITAR - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(
            int id,
            FilmeFormViewModel modelo,
            CancellationToken cancellationToken = default)
        {
            if (id != modelo.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await filmeService.PreencherImagemAtualAsync(
                    modelo, cancellationToken);

                return View(modelo);
            }

            var resultado = await filmeService.EditarAsync(
                modelo, cancellationToken);

            if (resultado.NaoEncontrado)
                return NotFound();

            if (resultado.ErroImagem is not null)
            {
                ModelState.AddModelError(
                    nameof(modelo.Imagem),
                    resultado.ErroImagem);

                await filmeService.PreencherImagemAtualAsync(
                    modelo, cancellationToken);

                return View(modelo);
            }

            return RedirectToAction(nameof(Index));
        }

        // EXCLUIR - GET
        [HttpGet]
        public async Task<IActionResult> Excluir(
            int? id,
            CancellationToken cancellationToken = default)
        {
            if (id is null)
                return NotFound();

            var filme = await filmeService.ObterAsync(
                id.Value, cancellationToken);

            if (filme is null)
                return NotFound();

            return View(filme);
        }

        // EXCLUIR - POST
        [HttpPost]
        [ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluirConfirmado(
            int id,
            CancellationToken cancellationToken = default)
        {
            await filmeService.ExcluirAsync(
                id, cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}
