namespace Nucleos.Application.Features.Gamificacao.Services;

public interface IGamificationEngine
{
    Task ProcessarAcaoAsync(Guid userId, string acao, Guid? nucleoId = null, CancellationToken ct = default);
    Task AddXp(Guid userId, int xpAmount, CancellationToken cancellationToken = default);
    Task AddEnergy(Guid userId, int energyAmount, CancellationToken cancellationToken = default);
}