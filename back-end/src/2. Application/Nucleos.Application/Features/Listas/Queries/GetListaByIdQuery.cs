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

public class GetListaByIdQuery : IRequest<ListaDto>
{
    public Guid Id { get; set; }
}

public class GetListaByIdQueryHandler : IRequestHandler<GetListaByIdQuery, ListaDto>
{
    private readonly IApplicationDbContext _context;
    public GetListaByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<ListaDto> Handle(GetListaByIdQuery request, CancellationToken ct)
    {
        var lista = await _context.Listas
                        .FirstOrDefaultAsync(l => l.Id == request.Id && l.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Lista), request.Id);

        return new ListaDto
        {
            Id = lista.Id,
            BlocoId = lista.BlocoId,
            Nome = lista.Nome,
            TipoLista = lista.TipoLista,
            CreatedAt = lista.CreatedAt
        };
    }
}