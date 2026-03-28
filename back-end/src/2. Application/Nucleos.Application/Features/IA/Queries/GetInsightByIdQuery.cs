using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.IA.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.IA.Queries;

public class GetInsightByIdQuery : IRequest<InsightDto>
{
    public Guid Id { get; set; }
}

public class GetInsightByIdQueryHandler : IRequestHandler<GetInsightByIdQuery, InsightDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetInsightByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<InsightDto> Handle(GetInsightByIdQuery request, CancellationToken ct)
    {
        var insight = await _context.AiInsights.FirstOrDefaultAsync(i => i.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(AIInsight), request.Id);

        if (insight.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        return new InsightDto
        {
            Id = insight.Id,
            InsightType = insight.InsightType,
            Mensagem = insight.Mensagem,
            Priority = insight.Priority,
            Applied = insight.Applied,
            CreatedAt = insight.CreatedAt
        };
    }
}
