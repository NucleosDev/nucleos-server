using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.ItensLista.DTOs;

namespace Nucleos.Application.Features.ItensLista.Queries;

public class GetItensByListaQuery : IRequest<List<ItemListaDto>> { public Guid ListaId { get; set; } }

public class GetItensByListaQueryHandler : IRequestHandler<GetItensByListaQuery, List<ItemListaDto>>
{
    private readonly IApplicationDbContext _context;
    public GetItensByListaQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ItemListaDto>> Handle(GetItensByListaQuery request, CancellationToken ct)
        => await _context.ItensLista
            .Where(i => i.ListaId == request.ListaId && i.DeletedAt == null)
            .Select(i => new ItemListaDto { Id = i.Id, ListaId = i.ListaId, CategoriaId = i.CategoriaId, Nome = i.Nome, Quantidade = i.Quantidade, ValorUnitario = i.ValorUnitario, ValorTotal = i.ValorTotal, Checked = i.Checked, CreatedAt = i.CreatedAt })
            .ToListAsync(ct);
}
