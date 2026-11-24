namespace WebFilmes.ViewModels
{
    public class FilmeIndexViewModel
    {
        public string? TermoBusca { get; set; }
        public IReadOnlyList<FilmeViewModel> Filmes { get; set; } = [];

        public bool TemBusca => !string.IsNullOrWhiteSpace(TermoBusca);
    }
}
