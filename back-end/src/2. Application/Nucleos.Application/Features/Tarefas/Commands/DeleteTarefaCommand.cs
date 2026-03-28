using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Tarefas.Commands;

public class DeleteTarefaCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteTarefaCommandHandler : IRequestHandler<DeleteTarefaCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteTarefaCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteTarefaCommand request, CancellationToken ct)
    {
        var tarefa = await _context.Tarefas
                         .FirstOrDefaultAsync(t => t.Id == request.Id && t.DeletedAt == null, ct)
                     ?? throw new NotFoundException(nameof(Tarefa), request.Id);

        tarefa.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}