using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Habitos.DTOs;

namespace Nucleos.Application.Features.Habitos.Queries;

public class GetHabitosByBlocoQuery : IRequest<List<HabitoDto>> { public Guid BlocoId { get; set; } }

public class GetHabitosByBlocoQueryHandler : IRequestHandler<GetHabitosByBlocoQuery, List<HabitoDto>>
{
    private readonly IApplicationDbContext _context;
    public GetHabitosByBlocoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<HabitoDto>> Handle(GetHabitosByBlocoQuery request, CancellationToken ct)
        => await _context.Habitos
            .Where(h => h.BlocoId == request.BlocoId)
            .Select(h => new HabitoDto { Id = h.Id, BlocoId = h.BlocoId, Nome = h.Nome, Frequencia = h.Frequencia, MetaVezes = h.MetaVezes, CreatedAt = h.CreatedAt })
            .ToListAsync(ct);
}
