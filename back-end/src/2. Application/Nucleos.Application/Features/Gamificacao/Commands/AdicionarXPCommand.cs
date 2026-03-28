using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Gamificacao.Commands;

public class AdicionarXPCommand : IRequest<int>
{
    public Guid UserId { get; set; }
    public Guid? NucleoId { get; set; }
    public int XpAmount { get; set; }
    public string Source { get; set; } = string.Empty;
}

public class AdicionarXPCommandHandler : IRequestHandler<AdicionarXPCommand, int>
{
    private readonly IApplicationDbContext _context;
    public AdicionarXPCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(AdicionarXPCommand request, CancellationToken ct)
    {
        var log = new XP_Log
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            NucleoId = request.NucleoId,
            XpAmount = request.XpAmount,
            Source = request.Source,
            CreatedAt = DateTime.UtcNow
        };
        await _context.XpLogs.AddAsync(log, ct);

        var level = await _context.UserLevels.FirstOrDefaultAsync(l => l.UserId == request.UserId, ct);
        if (level != null)
        {
            level.CurrentXp += request.XpAmount;
            level.TotalXpEarned += request.XpAmount;
            while (level.CurrentXp >= level.NextLevelXp)
            {
                level.CurrentXp -= level.NextLevelXp;
                level.Level++;
                level.NextLevelXp = (int)(level.NextLevelXp * 1.5);
            }
            level.UpdatedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(ct);
        return level?.Level ?? 1;
    }
}
