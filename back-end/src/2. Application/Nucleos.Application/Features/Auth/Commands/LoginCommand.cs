using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Auth.DTOs;

namespace Nucleos.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IJwtGenerator jwtGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 🔹 Validação básica
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-mail ou senha inválidos"
                };
            }

            var email = request.Email.Trim().ToLower();

            var user = await _context.Users
                .Include(u => u.Profile)
                .Include(u => u.Security)
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            // 🔹 Anti enumeração de usuário
            if (user == null)
            {
                // simula verificação pra evitar timing attack
                BCrypt.Net.BCrypt.Verify(request.Password, "$2a$11$C6UzMDM.H6dfI/f/IKcEeO5s7x1YgnPZXoqBYwygJyI072QtdgQXK");

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-mail ou senha inválidos"
                };
            }

            if (!user.Active)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Conta desativada"
                };
            }

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            // 🔹 Senha inválida
            if (!passwordValid)
            {
                if (user.Security != null)
                {
                    user.Security.FailedAttempts++;

                    if (user.Security.FailedAttempts >= 5)
                    {
                        user.Active = false;

                        await _context.SaveChangesAsync(cancellationToken);

                        return new AuthResponseDto
                        {
                            Success = false,
                            Message = "Conta bloqueada por excesso de tentativas"
                        };
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-mail ou senha inválidos"
                };
            }

            // 🔹 Login válido
            if (user.Security != null)
            {
                user.Security.FailedAttempts = 0;
                user.Security.LastLogin = DateTime.UtcNow;
            }

            var userRole = await _context.UserRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.UserId == user.Id, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var expiresIn = request.RememberMe
                ? DateTime.UtcNow.AddDays(7)
                : DateTime.UtcNow.AddHours(1);

            var token = _jwtGenerator.GenerateToken(
                user.Id,
                user.Email,
                userRole?.Role ?? "user"
            );

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login realizado com sucesso!",
                UserId = user.Id,
                Email = user.Email,
                FullName = user.Profile?.FullName ?? string.Empty,
                Cpf = user.Cpf,
                Token = token,
                ExpiresAt = expiresIn
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");

            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro ao fazer login",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}