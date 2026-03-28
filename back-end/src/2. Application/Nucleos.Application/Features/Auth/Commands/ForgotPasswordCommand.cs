using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Auth.Commands;

public class ForgotPasswordCommand : IRequest<bool> { public string Email { get; set; } = string.Empty; }

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public ForgotPasswordCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken ct)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, ct);
        if (user == null) return true; // não revelar se existe
        var reset = new PasswordReset
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = Guid.NewGuid().ToString("N"),
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            Used = false,
            CreatedAt = DateTime.UtcNow
        };
        await _context.PasswordResets.AddAsync(reset, ct);
        await _context.SaveChangesAsync(ct);
        // TODO: enviar email com reset.Token
        return true;
    }
}
