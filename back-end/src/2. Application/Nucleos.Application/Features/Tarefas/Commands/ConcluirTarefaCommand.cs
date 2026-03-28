using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using System.Threading;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Tarefas.Commands;

public class ConcluirTarefaCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class ConcluirTarefaCommandHandler : IRequestHandler<ConcluirTarefaCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IGamificationEngine _gamification;

    public ConcluirTarefaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IGamificationEngine gamification)
    {
        _context = context;
        _currentUser = currentUser;
        _gamification = gamification;
    }

    public async Task<Unit> Handle(ConcluirTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .Include(t => t.Bloco)
            .ThenInclude(b => b.Nucleo)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (tarefa == null)
            throw new NotFoundException(nameof(Tarefa), request.Id);

        var nucleo = tarefa.Bloco.Nucleo;
        if (nucleo.UserId != _currentUser.UserId && !await IsSharedWithWrite(nucleo.Id))
            throw new ForbiddenException();

        tarefa.Status = "concluida";
        tarefa.ConcluidaEm = DateTime.UtcNow;
        tarefa.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        await _gamification.AddXp(nucleo.UserId, 20, cancellationToken);
        await _gamification.AddEnergy(nucleo.UserId, 5, cancellationToken);

        // Disparar evento
        // await _mediator.Publish(new TarefaConcluidaEvent(tarefa));

        return Unit.Value;
    }

    private async Task<bool> IsSharedWithWrite(Guid nucleoId)
    {
        var userId = _currentUser.UserId.Value;
        return await _context.NucleoCompartilhamentos
            .AnyAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == userId && c.PermissionLevel == "edit");
    }
}