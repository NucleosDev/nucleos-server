using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Commands;

public class DeleteHabitoCommand : IRequest
{
    public Guid Id { get; set; }
}

public class DeleteHabitoCommandHandler : IRequestHandler<DeleteHabitoCommand>
{
    private readonly IApplicationDbContext _context;
    public DeleteHabitoCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task Handle(DeleteHabitoCommand request, CancellationToken ct)
    {
        var habito = await _context.Habitos
                         .FirstOrDefaultAsync(h => h.Id == request.Id, ct)
                     ?? throw new NotFoundException(nameof(Habito), request.Id);

        _context.Habitos.Remove(habito);
        await _context.SaveChangesAsync(ct);
    }
}