using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Blocos.DTOs;

namespace Nucleos.Application.Features.Blocos.Queries;

public class GetBlocosByNucleoQuery : IRequest<List<BlocoListDto>> { public Guid NucleoId { get; set; } }

public class GetBlocosByNucleoQueryHandler : IRequestHandler<GetBlocosByNucleoQuery, List<BlocoListDto>>
{
    private readonly IApplicationDbContext _context;
    public GetBlocosByNucleoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<BlocoListDto>> Handle(GetBlocosByNucleoQuery request, CancellationToken ct)
    {
        return await _context.Blocos
            .Where(b => b.NucleoId == request.NucleoId && b.DeletedAt == null)
            .OrderBy(b => b.Posicao)
            .Select(b => new BlocoListDto { Id = b.Id, Tipo = b.Tipo, Titulo = b.Titulo, Posicao = b.Posicao })
            .ToListAsync(ct);
    }
}
