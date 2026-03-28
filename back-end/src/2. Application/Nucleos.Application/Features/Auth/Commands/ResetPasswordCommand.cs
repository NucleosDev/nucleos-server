using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<bool>
{
    public string Token { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public ResetPasswordCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken ct)
    {
        var reset = await _context.PasswordResets.FirstOrDefaultAsync(r => r.Token == request.Token && !r.Used && r.ExpiresAt > DateTime.UtcNow, ct)
            ?? throw new BusinessRuleException("Token inválido ou expirado.");
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == reset.UserId, ct)
            ?? throw new NotFoundException("User", reset.UserId);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        reset.Used = true;
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
