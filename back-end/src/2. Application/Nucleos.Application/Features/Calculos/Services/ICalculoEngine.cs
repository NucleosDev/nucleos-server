using Nucleos.Application.Features.Calculos.Services;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Application.Features.Calculos.DTOs;

namespace Nucleos.Application.Features.Calculos.Services;

public interface ICalculoEngine
{
    Task<double> ExecutarAsync(Guid blocoId, string tipoOperacao, string campo, string? agruparPor = null, CancellationToken ct = default);
    Task<CalculoResultadoDto> Calcular(Guid configId, object dados, CancellationToken ct);
}
