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

namespace Nucleos.Application.Features.Listas.Commands;

public class UpdateListaCommand : IRequest<ListaDto>
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? TipoLista { get; set; }
}

public class UpdateListaCommandHandler : IRequestHandler<UpdateListaCommand, ListaDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateListaCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<ListaDto> Handle(UpdateListaCommand request, CancellationToken ct)
    {
        var lista = await _context.Listas
                        .FirstOrDefaultAsync(l => l.Id == request.Id && l.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Lista), request.Id);

        if (request.Nome != null) lista.Nome = request.Nome;
        if (request.TipoLista != null) lista.TipoLista = request.TipoLista;
        lista.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

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