namespace WebFilmes.Services
{
    public class ResultadoServico(bool Sucesso, bool NaoEncontrado = false, string? ErroImagem = null)
    {
        public bool  Sucesso { get; set; } = Sucesso;

        public bool NaoEncontrado { get; set; } = NaoEncontrado;

        public string? ErroImagem { get; set; } = ErroImagem;


        public static ResultadoServico Ok() => new(true);
        public static ResultadoServico FilmeNaoEncontrado() => new(false, NaoEncontrado: true);
        public static ResultadoServico ImagemInvalida(string erro) => new(false, ErroImagem: erro);
    }
}
