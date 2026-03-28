using MediatR;
using Nucleos.Application.Features.Gamificacao.Commands;
using Nucleos.Application.Features.Gamificacao.Services;

namespace Nucleos.Application.Features.Gamificacao.Services;

public class GamificationEngineService : IGamificationEngine
{
    private readonly IMediator _mediator;
    private static readonly Dictionary<string, int> _xpTable = new()
    {
        ["tarefa_concluida"] = 20,
        ["habito_registrado"] = 10,
        ["nucleo_criado"] = 50,
        ["bloco_criado"] = 15,
        ["lista_criada"] = 10,
        ["meta_concluida"] = 100
    };

    public GamificationEngineService(IMediator mediator) => _mediator = mediator;

    public async Task ProcessarAcaoAsync(Guid userId, string acao, Guid? nucleoId = null, CancellationToken ct = default)
    {
        if (_xpTable.TryGetValue(acao, out var xp))
            await _mediator.Send(new AdicionarXPCommand { UserId = userId, NucleoId = nucleoId, XpAmount = xp, Source = acao }, ct);
        await _mediator.Send(new AtualizarStreakCommand { UserId = userId, NucleoId = nucleoId }, ct);
    }

    public async Task AddXp(Guid userId, int xpAmount, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new AdicionarXPCommand { UserId = userId, XpAmount = xpAmount }, cancellationToken);
    }

    public async Task AddEnergy(Guid userId, int energyAmount, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Adicionar energia ainda não implementado.");
    }
}