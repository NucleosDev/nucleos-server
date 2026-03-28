using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.ItensLista.Commands;

public class DeleteItemCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteItemCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteItemCommand request, CancellationToken ct)
    {
        var item = await _context.ItensLista
                       .FirstOrDefaultAsync(i => i.Id == request.Id && i.DeletedAt == null, ct)
                   ?? throw new NotFoundException(nameof(ItemLista), request.Id);

        item.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}