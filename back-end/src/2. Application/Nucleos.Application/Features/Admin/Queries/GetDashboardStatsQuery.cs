using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Admin.DTOs;

namespace Nucleos.Application.Features.Admin.Queries;

public class GetDashboardStatsQuery : IRequest<DashboardStatsDto> { }

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly IApplicationDbContext _context;
    public GetDashboardStatsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken ct)
        => new DashboardStatsDto
        {
            TotalUsers = await _context.Users.CountAsync(ct),
            ActiveUsers = await _context.Users.CountAsync(u => u.Active, ct),
            TotalNucleos = await _context.Nucleos.CountAsync(n => n.DeletedAt == null, ct),
            TotalTarefas = await _context.Tarefas.CountAsync(t => t.DeletedAt == null, ct),
            TotalHabitos = await _context.Habitos.CountAsync(ct),
            GeneratedAt = DateTime.UtcNow
        };
}
