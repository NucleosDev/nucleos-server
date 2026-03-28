using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.Auth.Commands;

public class VerifyEmailCommand : IRequest<bool> { public Guid UserId { get; set; } }

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public VerifyEmailCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
        if (user == null) return false;
        user.EmailVerified = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
