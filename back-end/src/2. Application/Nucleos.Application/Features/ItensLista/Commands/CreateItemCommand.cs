using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.ItensLista.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.ItensLista.Commands;

public class CreateItemCommand : IRequest<ItemListaDto>
{
    public Guid ListaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal? Quantidade { get; set; }
    public decimal? ValorUnitario { get; set; }
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemListaDto>
{
    private readonly IApplicationDbContext _context;
    public CreateItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<ItemListaDto> Handle(CreateItemCommand request, CancellationToken ct)
    {
        var item = new ItemLista
        {
            Id = Guid.NewGuid(),
            ListaId = request.ListaId,
            CategoriaId = request.CategoriaId,
            Nome = request.Nome,
            Quantidade = request.Quantidade ?? 1,
            ValorUnitario = request.ValorUnitario,
            ValorTotal = (request.Quantidade ?? 1) * (request.ValorUnitario ?? 0),
            Checked = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _context.ItensLista.AddAsync(item, ct);
        await _context.SaveChangesAsync(ct);
        return new ItemListaDto { Id = item.Id, ListaId = item.ListaId, CategoriaId = item.CategoriaId, Nome = item.Nome, Quantidade = item.Quantidade, ValorUnitario = item.ValorUnitario, ValorTotal = item.ValorTotal, Checked = item.Checked, CreatedAt = item.CreatedAt };
    }
}
