using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Commands;

public class RegistrarHabitoCommand : IRequest<Unit>
{
    public Guid HabitoId { get; set; }
    public DateTime Data { get; set; }
    public int VezesCompletadas { get; set; } = 1;
}

public class RegistrarHabitoCommandHandler : IRequestHandler<RegistrarHabitoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IGamificationEngine _gamification;

    public RegistrarHabitoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IGamificationEngine gamification)
    {
        _context = context;
        _currentUser = currentUser;
        _gamification = gamification;
    }

    // Método auxiliar para verificar permissão de escrita
    private async Task<bool> HasWriteAccess(Guid nucleoId)
    {
        var compartilhamento = await _context.NucleoCompartilhamentos
            .FirstOrDefaultAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == _currentUser.UserId);
        return compartilhamento != null && compartilhamento.PermissionLevel == "edit";
    }

    public async Task<Unit> Handle(RegistrarHabitoCommand request, CancellationToken cancellationToken)
    {
        var habito = await _context.Habitos
            .Include(h => h.Bloco)
            .ThenInclude(b => b.Nucleo)
            .FirstOrDefaultAsync(h => h.Id == request.HabitoId, cancellationToken);

        if (habito == null)
            throw new NotFoundException(nameof(Habito), request.HabitoId);

        var nucleo = habito.Bloco.Nucleo;
        if (nucleo.UserId != _currentUser.UserId && !await HasWriteAccess(nucleo.Id))
            throw new ForbiddenException();

        var registro = new HabitoRegistro
        {
            Id = Guid.NewGuid(),
            HabitoId = request.HabitoId,
            Data = request.Data.Date,
            VezesCompletadas = request.VezesCompletadas,
            CreatedAt = DateTime.UtcNow
        };

        await _context.HabitosRegistros.AddAsync(registro, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Adiciona XP (assumindo que IGamificationEngine tem AddXp(userId, xp, cancellationToken))
        int xpGanho = 5 * request.VezesCompletadas;
        await _gamification.AddXp(nucleo.UserId, xpGanho, cancellationToken);

        // Disparar evento (opcional, pode ser implementado depois)
        // await _mediator.Publish(new HabitoRegistradoEvent(habito, registro));

        return Unit.Value;
    }
}