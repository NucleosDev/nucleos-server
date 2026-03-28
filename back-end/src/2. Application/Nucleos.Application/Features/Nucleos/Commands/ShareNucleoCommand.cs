using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Nucleos.Commands;

public class ShareNucleoCommand : IRequest<Unit>
{
    public Guid NucleoId { get; set; }
    public string UserEmail { get; set; }
    public string PermissionLevel { get; set; } = "view";
}

public class ShareNucleoCommandHandler : IRequestHandler<ShareNucleoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ShareNucleoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ShareNucleoCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == request.NucleoId && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var userToShare = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.UserEmail, cancellationToken);

        if (userToShare == null)
            throw new NotFoundException(nameof(User), request.UserEmail);

        var existing = await _context.NucleoCompartilhamentos
            .FirstOrDefaultAsync(c => c.NucleoId == request.NucleoId && c.SharedWithUserId == userToShare.Id, cancellationToken);

        if (existing != null)
        {
            existing.PermissionLevel = request.PermissionLevel;
            existing.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var compartilhamento = new NucleoCompartilhamento
            {
                Id = Guid.NewGuid(),
                NucleoId = request.NucleoId,
                OwnerUserId = _currentUser.UserId.Value,
                SharedWithUserId = userToShare.Id,
                PermissionLevel = request.PermissionLevel,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _context.NucleoCompartilhamentos.AddAsync(compartilhamento, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}