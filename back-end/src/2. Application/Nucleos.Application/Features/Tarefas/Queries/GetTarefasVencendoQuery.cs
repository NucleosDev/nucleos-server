using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Tarefas.DTOs;

namespace Nucleos.Application.Features.Tarefas.Queries;

public class GetTarefasVencendoQuery : IRequest<List<TarefaDto>>
{
    public Guid UserId { get; set; }
    public int DiasAntecedencia { get; set; } = 3;
}

public class GetTarefasVencendoQueryHandler : IRequestHandler<GetTarefasVencendoQuery, List<TarefaDto>>
{
    private readonly IApplicationDbContext _context;
    public GetTarefasVencendoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<TarefaDto>> Handle(GetTarefasVencendoQuery request, CancellationToken ct)
    {
        var limite = DateTime.UtcNow.AddDays(request.DiasAntecedencia);
        var nucleoIds = await _context.Nucleos.Where(n => n.UserId == request.UserId && n.DeletedAt == null).Select(n => n.Id).ToListAsync(ct);
        var blocoIds = await _context.Blocos.Where(b => nucleoIds.Contains(b.NucleoId) && b.DeletedAt == null).Select(b => b.Id).ToListAsync(ct);
        return await _context.Tarefas
            .Where(t => blocoIds.Contains(t.BlocoId) && t.DataVencimento <= limite && t.Status != "concluida" && t.DeletedAt == null)
            .Select(t => new TarefaDto { Id = t.Id, BlocoId = t.BlocoId, Titulo = t.Titulo, Prioridade = t.Prioridade, Status = t.Status, DataVencimento = t.DataVencimento, CreatedAt = t.CreatedAt })
            .ToListAsync(ct);
    }
}
