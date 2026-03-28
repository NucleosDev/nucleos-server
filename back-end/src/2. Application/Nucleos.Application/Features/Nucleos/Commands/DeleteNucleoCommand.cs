using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using System.Threading;
using Nucleos.Domain.Entities;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Nucleos.Commands;

public class DeleteNucleoCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteNucleoCommandHandler : IRequestHandler<DeleteNucleoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteNucleoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(DeleteNucleoCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.Id);

        nucleo.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}