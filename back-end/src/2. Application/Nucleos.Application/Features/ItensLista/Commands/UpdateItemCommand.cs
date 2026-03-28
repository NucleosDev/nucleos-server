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

public class UpdateItemCommand : IRequest<ItemListaDto>
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public decimal? Quantidade { get; set; }
    public decimal? ValorUnitario { get; set; }
    public bool? Checked { get; set; }
    public Guid? CategoriaId { get; set; }
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemListaDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<ItemListaDto> Handle(UpdateItemCommand request, CancellationToken ct)
    {
        var item = await _context.ItensLista
            .FirstOrDefaultAsync(i => i.Id == request.Id && i.DeletedAt == null, ct)
            ?? throw new NotFoundException(nameof(ItemLista), request.Id);

        if (request.Nome != null) item.Nome = request.Nome;
        if (request.Quantidade.HasValue) item.Quantidade = request.Quantidade.Value;
        if (request.ValorUnitario.HasValue) item.ValorUnitario = request.ValorUnitario.Value;
        if (request.Checked.HasValue) item.Checked = request.Checked.Value;
        if (request.CategoriaId.HasValue) item.CategoriaId = request.CategoriaId.Value;

        // Atualizar o campo calculado manualmente, se necessário
        item.ValorTotal = item.Quantidade * item.ValorUnitario;

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