namespace WebFilmes.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;       
        public string Genero { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        public decimal Nota { get; set; }
        public string Sinopse { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;

    }
}
