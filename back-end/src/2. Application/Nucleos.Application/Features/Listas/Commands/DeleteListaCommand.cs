using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Listas.Commands;

public class DeleteListaCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteListaCommandHandler : IRequestHandler<DeleteListaCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteListaCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteListaCommand request, CancellationToken ct)
    {
        var lista = await _context.Listas
                        .FirstOrDefaultAsync(l => l.Id == request.Id && l.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Lista), request.Id);

        lista.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}