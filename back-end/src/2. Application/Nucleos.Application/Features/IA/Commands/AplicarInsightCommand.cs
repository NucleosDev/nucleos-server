using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.IA.Commands;

public class AplicarInsightCommand : IRequest<bool>
{
    public Guid InsightId { get; set; }
    public Guid Id { get => InsightId; set => InsightId = value; }
}

public class AplicarInsightCommandHandler : IRequestHandler<AplicarInsightCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public AplicarInsightCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(AplicarInsightCommand request, CancellationToken ct)
    {
        var insight = await _context.AiInsights.FirstOrDefaultAsync(i => i.Id == request.InsightId, ct)
            ?? throw new NotFoundException("AIInsight", request.InsightId);
        insight.Applied = true;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
