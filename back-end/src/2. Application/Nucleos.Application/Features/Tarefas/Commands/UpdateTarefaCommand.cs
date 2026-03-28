using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Tarefas.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Tarefas.Commands;

public class UpdateTarefaCommand : IRequest<TarefaDto>
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Prioridade { get; set; }
    public string? Status { get; set; }
    public DateTime? DataVencimento { get; set; }
}

public class UpdateTarefaCommandHandler : IRequestHandler<UpdateTarefaCommand, TarefaDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateTarefaCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<TarefaDto> Handle(UpdateTarefaCommand request, CancellationToken ct)
    {
        var tarefa = await _context.Tarefas
                         .FirstOrDefaultAsync(t => t.Id == request.Id && t.DeletedAt == null, ct)
                     ?? throw new NotFoundException(nameof(Tarefa), request.Id);

        tarefa.Titulo = request.Titulo;
        tarefa.Descricao = request.Descricao;
        if (request.Prioridade != null) tarefa.Prioridade = request.Prioridade;
        if (request.Status != null) tarefa.Status = request.Status;
        tarefa.DataVencimento = request.DataVencimento;
        tarefa.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return new TarefaDto
        {
            Id = tarefa.Id,
            BlocoId = tarefa.BlocoId,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            Prioridade = tarefa.Prioridade,
            Status = tarefa.Status,
            DataVencimento = tarefa.DataVencimento,
            CreatedAt = tarefa.CreatedAt
        };
    }
}