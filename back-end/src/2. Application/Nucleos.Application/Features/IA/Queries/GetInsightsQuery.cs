using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.IA.DTOs;

namespace Nucleos.Application.Features.IA.Queries;

public class GetInsightsQuery : IRequest<List<InsightDto>>
{
    public Guid UserId { get; set; }
    public bool OnlyPending { get; set; } = false;
}

public class GetInsightsQueryHandler : IRequestHandler<GetInsightsQuery, List<InsightDto>>
{
    private readonly IApplicationDbContext _context;
    public GetInsightsQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<InsightDto>> Handle(GetInsightsQuery request, CancellationToken ct)
    {
        var query = _context.AiInsights.Where(i => i.UserId == request.UserId);
        if (request.OnlyPending) query = query.Where(i => !i.Applied);
        return await query.OrderByDescending(i => i.Priority).ThenByDescending(i => i.CreatedAt)
            .Select(i => new InsightDto { Id = i.Id, InsightType = i.InsightType, Mensagem = i.Mensagem, Priority = i.Priority, Applied = i.Applied, CreatedAt = i.CreatedAt })
            .ToListAsync(ct);
    }
}
