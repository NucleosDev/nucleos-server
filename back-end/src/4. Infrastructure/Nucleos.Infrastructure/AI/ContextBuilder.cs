using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.AI;

public class ContextBuilder
{
    private readonly IApplicationDbContext _context;
    public ContextBuilder(IApplicationDbContext context) => _context = context;

    public async Task<string> BuildAsync(Guid userId, CancellationToken ct = default)
    {
        var nucleos = await _context.Nucleos.Where(n => n.UserId == userId && n.DeletedAt == null).CountAsync(ct);
        var pendentes = await _context.Tarefas.CountAsync(t => t.Status == "pendente" && t.DeletedAt == null, ct);
        return $"Usuário tem {nucleos} núcleos e {pendentes} tarefas pendentes.";
    }
}
