using System.ComponentModel.DataAnnotations;

namespace WebFilmes.ViewModels
{
    public class FilmeFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o título.")]
        [StringLength(150, ErrorMessage = "O título deve ter no máximo 150 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o gênero.")]
        [StringLength(80, ErrorMessage = "O gênero deve ter no máximo 80 caracteres.")]
        [Display(Name = "Gênero")]
        public string Genero { get; set; } = string.Empty;

        [Range(1888, 2100, ErrorMessage = "Informe um ano entre 1888 e 2100.")]
        [Display(Name = "Ano de lançamento")]
        public int AnoLancamento { get; set; }

        [Range(typeof(decimal), "0", "10", ErrorMessage = "A nota deve estar entre 0 e 10.")]
        [Display(Name = "Nota (0 a 10)")]
        public decimal Nota { get; set; }

        [StringLength(1000, ErrorMessage = "A sinopse deve ter no máximo 1000 caracteres.")]
        [Display(Name = "Sinopse")]
        public string? Sinopse { get; set; }

        [Display(Name = "Imagem do filme")]
        public IFormFile? Imagem { get; set; }

        // Usada apenas para exibir a imagem atual; nunca vem do formulário.
        public string? ImagemUrl { get; set; }
    }
}
