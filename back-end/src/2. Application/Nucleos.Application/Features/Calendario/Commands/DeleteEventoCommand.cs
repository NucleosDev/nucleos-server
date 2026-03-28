using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calendario.Commands;

public class DeleteEventoCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteEventoCommandHandler : IRequestHandler<DeleteEventoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteEventoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteEventoCommand request, CancellationToken ct)
    {
        var evento = await _context.CalendarioEventos
            .Include(e => e.Nucleo)
            .FirstOrDefaultAsync(e => e.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(CalendarioEvento), request.Id);

        if (evento.Nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        _context.CalendarioEventos.Remove(evento);
        await _context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
