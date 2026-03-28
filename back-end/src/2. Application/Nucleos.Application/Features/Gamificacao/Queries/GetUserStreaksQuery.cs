using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Gamificacao.DTOs;

namespace Nucleos.Application.Features.Gamificacao.Queries;

public class GetUserStreaksQuery : IRequest<List<StreakDto>> { public Guid UserId { get; set; } }

public class GetUserStreaksQueryHandler : IRequestHandler<GetUserStreaksQuery, List<StreakDto>>
{
    private readonly IApplicationDbContext _context;
    public GetUserStreaksQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<StreakDto>> Handle(GetUserStreaksQuery request, CancellationToken ct)
        => await _context.Streaks
            .Where(s => s.UserId == request.UserId)
            .Select(s => new StreakDto { Id = s.Id, NucleoId = s.NucleoId, StreakType = s.StreakType, CurrentStreak = s.CurrentStreak, MaxStreak = s.MaxStreak, LastActivityDate = s.LastActivityDate })
            .ToListAsync(ct);
}
