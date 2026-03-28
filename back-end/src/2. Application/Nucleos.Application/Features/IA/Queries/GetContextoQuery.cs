using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.IA.Queries;

public class GetContextoQuery : IRequest<string> { public Guid UserId { get; set; } }

public class GetContextoQueryHandler : IRequestHandler<GetContextoQuery, string>
{
    private readonly IApplicationDbContext _context;
    public GetContextoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<string> Handle(GetContextoQuery request, CancellationToken ct)
    {
        var ctx = await _context.AiContext.FirstOrDefaultAsync(c => c.UserId == request.UserId, ct);
        return ctx?.LastSummary ?? string.Empty;
    }
}
