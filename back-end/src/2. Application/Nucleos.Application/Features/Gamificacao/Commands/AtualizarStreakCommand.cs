using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Gamificacao.Commands;

public class AtualizarStreakCommand : IRequest<int>
{
    public Guid UserId { get; set; }
    public Guid? NucleoId { get; set; }
    public string StreakType { get; set; } = "daily";
}

public class AtualizarStreakCommandHandler : IRequestHandler<AtualizarStreakCommand, int>
{
    private readonly IApplicationDbContext _context;
    public AtualizarStreakCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<int> Handle(AtualizarStreakCommand request, CancellationToken ct)
    {
        var streak = await _context.Streaks.FirstOrDefaultAsync(s => s.UserId == request.UserId && s.StreakType == request.StreakType && s.NucleoId == request.NucleoId, ct);
        var today = DateTime.UtcNow.Date;
        if (streak == null)
        {
            streak = new Streak { Id = Guid.NewGuid(), UserId = request.UserId, NucleoId = request.NucleoId, StreakType = request.StreakType, CurrentStreak = 1, MaxStreak = 1, LastActivityDate = today, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await _context.Streaks.AddAsync(streak, ct);
        }
        else
        {
            var lastDate = streak.LastActivityDate?.Date;
            if (lastDate == today) return streak.CurrentStreak;
            streak.CurrentStreak = (lastDate == today.AddDays(-1)) ? streak.CurrentStreak + 1 : 1;
            if (streak.CurrentStreak > streak.MaxStreak) streak.MaxStreak = streak.CurrentStreak;
            streak.LastActivityDate = today;
            streak.UpdatedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(ct);
        return streak.CurrentStreak;
    }
}
