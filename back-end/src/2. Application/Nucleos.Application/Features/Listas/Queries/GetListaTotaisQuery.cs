using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Listas.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Listas.Queries;

public class GetListaTotaisQuery : IRequest<ListaTotalDto>
{
    public Guid ListaId { get; set; }
}

public class GetListaTotaisQueryHandler : IRequestHandler<GetListaTotaisQuery, ListaTotalDto>
{
    private readonly IApplicationDbContext _context;
    public GetListaTotaisQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ListaTotalDto> Handle(GetListaTotaisQuery request, CancellationToken ct)
    {
        var lista = await _context.Listas
                        .FirstOrDefaultAsync(l => l.Id == request.ListaId && l.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Lista), request.ListaId);

        var itens = await _context.ItensLista
            .Where(i => i.ListaId == request.ListaId && i.DeletedAt == null)
            .ToListAsync(ct);

        var total = itens.Sum(i => i.ValorTotal ?? 0);
        var quantidade = itens.Count;

        return new ListaTotalDto
        {
            ListaId = lista.Id,
            Nome = lista.Nome,
            Total = total,
            QuantidadeItens = quantidade
        };
    }
}