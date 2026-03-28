using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Tarefas.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Tarefas.Commands;

public class CreateTarefaCommand : IRequest<TarefaDto>
{
    public Guid BlocoId { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Prioridade { get; set; } = "media";
    public DateTime? DataVencimento { get; set; }
    public int Posicao { get; set; }
}

public class CreateTarefaCommandHandler : IRequestHandler<CreateTarefaCommand, TarefaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateTarefaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<TarefaDto> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos.FindAsync(request.BlocoId);
        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.BlocoId);

        // Verifica permissão
        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId && !await IsSharedWithWrite(nucleo.Id))
            throw new ForbiddenException();

        var tarefa = new Tarefa
        {
            Id = Guid.NewGuid(),
            BlocoId = request.BlocoId,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            Prioridade = request.Prioridade,
            DataVencimento = request.DataVencimento,
            Posicao = request.Posicao,
            Status = "pendente",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Tarefas.AddAsync(tarefa, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TarefaDto>(tarefa);
    }

    private async Task<bool> IsSharedWithWrite(Guid nucleoId)
    {
        var userId = _currentUser.UserId.Value;
        return await _context.NucleoCompartilhamentos
            .AnyAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == userId && c.PermissionLevel == "edit");
    }
}