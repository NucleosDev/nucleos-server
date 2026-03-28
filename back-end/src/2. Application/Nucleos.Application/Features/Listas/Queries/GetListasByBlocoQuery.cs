using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Listas.DTOs;

namespace Nucleos.Application.Features.Listas.Queries;

public class GetListasByBlocoQuery : IRequest<List<ListaDto>> { public Guid BlocoId { get; set; } }

public class GetListasByBlocoQueryHandler : IRequestHandler<GetListasByBlocoQuery, List<ListaDto>>
{
    private readonly IApplicationDbContext _context;
    public GetListasByBlocoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ListaDto>> Handle(GetListasByBlocoQuery request, CancellationToken ct)
        => await _context.Listas
            .Where(l => l.BlocoId == request.BlocoId && l.DeletedAt == null)
            .Select(l => new ListaDto { Id = l.Id, BlocoId = l.BlocoId, Nome = l.Nome, TipoLista = l.TipoLista, CreatedAt = l.CreatedAt })
            .ToListAsync(ct);
}
