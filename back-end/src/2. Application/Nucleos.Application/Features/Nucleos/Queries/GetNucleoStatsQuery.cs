using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Nucleos.DTOs;

namespace Nucleos.Application.Features.Nucleos.Queries;

public class GetNucleoStatsQuery : IRequest<NucleoStatsDto>
{
    public Guid NucleoId { get; set; }
}

public class GetNucleoStatsQueryHandler : IRequestHandler<GetNucleoStatsQuery, NucleoStatsDto>
{
    private readonly IApplicationDbContext _context;
    public GetNucleoStatsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<NucleoStatsDto> Handle(GetNucleoStatsQuery request, CancellationToken ct)
    {
        var blocoIds = await _context.Blocos
            .Where(b => b.NucleoId == request.NucleoId && b.DeletedAt == null)
            .Select(b => b.Id).ToListAsync(ct);

        var totalTarefas = await _context.Tarefas.CountAsync(t => blocoIds.Contains(t.BlocoId) && t.DeletedAt == null, ct);
        var concluidas = await _context.Tarefas.CountAsync(t => blocoIds.Contains(t.BlocoId) && t.Status == "concluida" && t.DeletedAt == null, ct);
        var totalHabitos = await _context.Habitos.CountAsync(h => blocoIds.Contains(h.BlocoId), ct);
        var xpTotal = await _context.XpLogs.Where(x => x.NucleoId == request.NucleoId).SumAsync(x => x.XpAmount, ct);

        return new NucleoStatsDto
        {
            NucleoId = request.NucleoId,
            TotalBlocos = blocoIds.Count,
            TotalTarefas = totalTarefas,
            TarefasConcluidas = concluidas,
            TotalHabitos = totalHabitos,
            TotalXP = xpTotal
        };
    }
}
