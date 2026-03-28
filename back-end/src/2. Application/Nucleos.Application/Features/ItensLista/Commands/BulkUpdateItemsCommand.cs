using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.ItensLista.Commands;

public class BulkUpdateItemsCommand : IRequest
{
    public List<Guid> Ids { get; set; } = new();
    public bool? Checked { get; set; }
}

public class BulkUpdateItemsCommandHandler : IRequestHandler<BulkUpdateItemsCommand>
{
    private readonly IApplicationDbContext _context;
    public BulkUpdateItemsCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(BulkUpdateItemsCommand request, CancellationToken ct)
    {
        var itens = await _context.ItensLista.Where(i => request.Ids.Contains(i.Id) && i.DeletedAt == null).ToListAsync(ct);
        foreach (var item in itens)
        {
            if (request.Checked.HasValue) item.Checked = request.Checked.Value;
            item.UpdatedAt = DateTime.UtcNow;
        }
        await _context.SaveChangesAsync(ct);
    }
}
