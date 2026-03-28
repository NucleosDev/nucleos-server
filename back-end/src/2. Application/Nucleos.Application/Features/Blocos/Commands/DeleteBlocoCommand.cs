using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.Blocos.Commands;

public class DeleteBlocoCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteBlocoCommandHandler : IRequestHandler<DeleteBlocoCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteBlocoCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteBlocoCommand request, CancellationToken ct)
    {
        var bloco = await _context.Blocos
                        .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Bloco), request.Id);

        bloco.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}