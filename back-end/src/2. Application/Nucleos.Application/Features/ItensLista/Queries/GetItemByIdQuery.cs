using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.ItensLista.DTOs;

namespace Nucleos.Application.Features.ItensLista.Queries;

public class GetItemByIdQuery : IRequest<ItemListaDto> { public Guid Id { get; set; } }

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemListaDto>
{
    private readonly IApplicationDbContext _context;
    public GetItemByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ItemListaDto> Handle(GetItemByIdQuery request, CancellationToken ct)
    {
        var i = await _context.ItensLista.FirstOrDefaultAsync(x => x.Id == request.Id && x.DeletedAt == null, ct)
            ?? throw new NotFoundException("ItemLista", request.Id);
        return new ItemListaDto { Id = i.Id, ListaId = i.ListaId, CategoriaId = i.CategoriaId, Nome = i.Nome, Quantidade = i.Quantidade, ValorUnitario = i.ValorUnitario, ValorTotal = i.ValorTotal, Checked = i.Checked, CreatedAt = i.CreatedAt };
    }
}
