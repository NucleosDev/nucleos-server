using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.ItensLista.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.ItensLista.Commands;

public class ToggleItemCheckedCommand : IRequest<ItemListaDto>
{
    public Guid Id { get; set; }
}

public class ToggleItemCheckedCommandHandler : IRequestHandler<ToggleItemCheckedCommand, ItemListaDto>
{
    private readonly IApplicationDbContext _context;
    public ToggleItemCheckedCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<ItemListaDto> Handle(ToggleItemCheckedCommand request, CancellationToken ct)
    {
        var item = await _context.ItensLista
                       .FirstOrDefaultAsync(i => i.Id == request.Id && i.DeletedAt == null, ct)
                   ?? throw new NotFoundException(nameof(ItemLista), request.Id);

        item.Checked = !item.Checked;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        return new ItemListaDto
        {
            Id = item.Id,
            ListaId = item.ListaId,
            Nome = item.Nome,
            Quantidade = item.Quantidade,
            ValorUnitario = item.ValorUnitario,
            ValorTotal = item.ValorTotal,
            Checked = item.Checked,
            CategoriaId = item.CategoriaId,
            CreatedAt = item.CreatedAt
        };
    }
}