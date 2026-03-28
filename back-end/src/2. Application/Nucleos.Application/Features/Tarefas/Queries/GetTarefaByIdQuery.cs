using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Tarefas.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Tarefas.Queries;

public class GetTarefaByIdQuery : IRequest<TarefaDto>
{
    public Guid Id { get; set; }
}

public class GetTarefaByIdQueryHandler : IRequestHandler<GetTarefaByIdQuery, TarefaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetTarefaByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<TarefaDto> Handle(GetTarefaByIdQuery request, CancellationToken ct)
    {
        var tarefa = await _context.Tarefas
            .Include(t => t.Bloco)
            .ThenInclude(b => b.Nucleo)
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.DeletedAt == null, ct)
            ?? throw new NotFoundException(nameof(Tarefa), request.Id);

        var nucleo = tarefa.Bloco.Nucleo;
        if (nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        return _mapper.Map<TarefaDto>(tarefa);
    }
}
