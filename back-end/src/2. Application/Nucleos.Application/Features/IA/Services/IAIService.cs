namespace Nucleos.Application.Features.IA.Services;

public interface IAIService
{
    Task<string> GerarInsightAsync(string contexto, CancellationToken ct = default);
    Task<string> EnviarMensagemAsync(string mensagem, string? historico = null, CancellationToken ct = default);
}
