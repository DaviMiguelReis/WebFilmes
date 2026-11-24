namespace WebFilmes.Services
{
    public class ImagemService(IWebHostEnvironment environment,
    ILogger<ImagemService> logger)
    {
        private const long TamanhoMaximo = 5 * 1024 * 1024;
        private const string Pasta = "imagens";
        private const string Prefixo = $"~/{Pasta}/";

        public async Task<(string? Extensao, string? Erro)> ValidarAsync(
            IFormFile? imagem,
            CancellationToken cancellationToken)
        {
            if (imagem is null || imagem.Length == 0)
            {
                return (null, null);
            }

            if (imagem.Length > TamanhoMaximo)
            {
                return (null, "A imagem deve ter no máximo 5 MB.");
            }

            var cabecalho = new byte[12];
            await using var stream = imagem.OpenReadStream();
            var bytesLidos = await stream.ReadAsync(cabecalho.AsMemory(), cancellationToken);

            if (bytesLidos >= 8 &&
                cabecalho.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }))
            {
                return (".png", null);
            }

            if (bytesLidos >= 3 && cabecalho[0] == 0xFF && cabecalho[1] == 0xD8 && cabecalho[2] == 0xFF)
            {
                return (".jpg", null);
            }

            if (bytesLidos >= 12 &&
                cabecalho.AsSpan(0, 4).SequenceEqual("RIFF"u8) &&
                cabecalho.AsSpan(8, 4).SequenceEqual("WEBP"u8))
            {
                return (".webp", null);
            }

            return (null, "Envie uma imagem PNG, JPG ou WEBP válida.");
        }

        public async Task<string> SalvarAsync(
            IFormFile imagem,
            string extensao,
            CancellationToken cancellationToken)
        {
            var nomeArquivo = $"{Guid.NewGuid():N}{extensao}";
            var diretorio = Path.Combine(environment.WebRootPath, Pasta);
            Directory.CreateDirectory(diretorio);

            var caminho = Path.Combine(diretorio, nomeArquivo);
            var url = Prefixo + nomeArquivo;
            try
            {
                await using var stream = new FileStream(
                    caminho,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None);
                await imagem.CopyToAsync(stream, cancellationToken);
            }
            catch
            {
                Excluir(url);
                throw;
            }

            return url;
        }

        public void Excluir(string? imagemUrl)
        {
            if (imagemUrl is null || !imagemUrl.StartsWith(Prefixo, StringComparison.Ordinal))
            {
                return;
            }

            var nomeArquivo = imagemUrl[Prefixo.Length..];
            if (Path.GetFileName(nomeArquivo) != nomeArquivo)
            {
                return;
            }

            var caminho = Path.Combine(environment.WebRootPath, Pasta, nomeArquivo);
            try
            {
                if (File.Exists(caminho))
                {
                    File.Delete(caminho);
                }
            }
            catch (IOException excecao)
            {
                logger.LogWarning(excecao, "Não foi possível remover a imagem {ImagemUrl}.", imagemUrl);
            }
            catch (UnauthorizedAccessException excecao)
            {
                logger.LogWarning(excecao, "Sem permissão para remover a imagem {ImagemUrl}.", imagemUrl);
            }
        }
    }
}
