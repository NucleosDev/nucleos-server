using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Gamification;

public class GamificationEngine : IGamificationEngine
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public GamificationEngine(IApplicationDbContext context, IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task ProcessarAcaoAsync(Guid userId, string acao, Guid? nucleoId = null, CancellationToken ct = default)
    {
        var xp = acao switch
        {
            "tarefa_concluida" => 20,
            "habito_registrado" => 10,
            "nucleo_criado" => 50,
            "bloco_criado" => 15,
            "lista_criada" => 10,
            "meta_concluida" => 100,
            _ => 5
        };
        await AddXpInternal(userId, nucleoId, xp, acao, ct);
    }

    public Task AddXp(Guid userId, int xpAmount, CancellationToken cancellationToken = default)
        => AddXpInternal(userId, null, xpAmount, "manual", cancellationToken);

    public async Task AddEnergy(Guid userId, int energyAmount, CancellationToken cancellationToken = default)
    {
        var energyLog = new EnergyLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NucleoId = null,
            EnergyAmount = energyAmount,
            CreatedAt = _dateTime.UtcNow
        };
        await _context.EnergyLogs.AddAsync(energyLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task AddXpInternal(Guid userId, Guid? nucleoId, int amount, string source, CancellationToken ct)
    {
        var level = await _context.UserLevels.FirstOrDefaultAsync(l => l.UserId == userId, ct);
        if (level == null)
        {
            level = new UserLevel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Level = 1,
                CurrentXp = amount,
                NextLevelXp = 100,
                TotalXpEarned = amount,
                UpdatedAt = _dateTime.UtcNow
            };
            await _context.UserLevels.AddAsync(level, ct);
        }
        else
        {
            level.CurrentXp += amount;
            level.TotalXpEarned += amount;
            while (level.CurrentXp >= level.NextLevelXp)
            {
                level.Level++;
                level.CurrentXp -= level.NextLevelXp;
                level.NextLevelXp = (int)(level.NextLevelXp * 1.2);
            }
            level.UpdatedAt = _dateTime.UtcNow;
        }

        var xpLog = new XP_Log
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NucleoId = nucleoId,
            XpAmount = amount,
            Source = source,
            CreatedAt = _dateTime.UtcNow
        };
        await _context.XpLogs.AddAsync(xpLog, ct);

        await _context.SaveChangesAsync(ct);
    }
}
