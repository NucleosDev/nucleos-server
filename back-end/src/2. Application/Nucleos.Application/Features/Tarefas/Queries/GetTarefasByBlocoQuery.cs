using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Tarefas.DTOs;

namespace Nucleos.Application.Features.Tarefas.Queries;

public class GetTarefasByBlocoQuery : IRequest<List<TarefaDto>> { public Guid BlocoId { get; set; } }

public class GetTarefasByBlocoQueryHandler : IRequestHandler<GetTarefasByBlocoQuery, List<TarefaDto>>
{
    private readonly IApplicationDbContext _context;
    public GetTarefasByBlocoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<TarefaDto>> Handle(GetTarefasByBlocoQuery request, CancellationToken ct)
        => await _context.Tarefas
            .Where(t => t.BlocoId == request.BlocoId && t.DeletedAt == null)
            .OrderBy(t => t.Posicao)
            .Select(t => new TarefaDto { Id = t.Id, BlocoId = t.BlocoId, Titulo = t.Titulo, Descricao = t.Descricao, Prioridade = t.Prioridade, Status = t.Status, DataVencimento = t.DataVencimento, ConcluidaEm = t.ConcluidaEm, Posicao = t.Posicao, CreatedAt = t.CreatedAt })
            .ToListAsync(ct);
}
