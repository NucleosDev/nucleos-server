using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Auth.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string? Nickname { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IApplicationDbContext context,
        IJwtGenerator jwtGenerator,
        ILogger<RegisterCommandHandler> logger)
    {
        _context = context;
        _jwtGenerator = jwtGenerator;
        _logger = logger;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Cpf))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Dados obrigatórios não informados"
                };
            }

            if (request.Password != request.ConfirmPassword)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "As senhas não coincidem"
                };
            }

            if (request.Password.Length < 6)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "A senha deve ter pelo menos 6 caracteres"
                };
            }

            // Normalizações
            var email = request.Email.Trim().ToLower();
            var cleanCpf = new string(request.Cpf.Where(char.IsDigit).ToArray());

            if (cleanCpf.Length != 11)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "CPF inválido"
                };
            }

            // Verificações de duplicidade
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == email, cancellationToken);

            if (emailExists)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-mail já cadastrado"
                };
            }

            var cpfExists = await _context.Users
                .AnyAsync(u => u.Cpf == cleanCpf, cancellationToken);

            if (cpfExists)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "CPF já cadastrado"
                };
            }

            // Hash da senha
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Criação do usuário
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Phone = request.Phone,
                Cpf = cleanCpf,
                PasswordHash = passwordHash,
                Active = true,
                EmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Nickname seguro
            var nickname = string.IsNullOrWhiteSpace(request.Nickname)
                ? request.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]
                : request.Nickname.Trim();

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FullName = request.FullName.Trim(),
                Nickname = nickname,
                AvatarUrl = $"https://ui-avatars.com/api/?background=random&color=fff&name={Uri.EscapeDataString(request.FullName)}",
                CreatedAt = DateTime.UtcNow
            };

            var userSecurity = new UserSecurity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FailedAttempts = 0,
                PasswordUpdatedAt = DateTime.UtcNow
            };

            var userRole = new UserRole
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Role = "user"
            };

            var userPreference = new UserPreference
            {
                UserId = user.Id,
                Theme = "system",
                Language = "pt-BR",
                Notifications = "{\"push\":true,\"email\":true,\"streaks\":true}",
                Shortcuts = "{}",
                DashboardLayout = "{}",
                UpdatedAt = DateTime.UtcNow
            };

            var userLevel = new UserLevel
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Level = 1,
                CurrentXp = 0,
                NextLevelXp = 100,
                TotalXpEarned = 0,
                UpdatedAt = DateTime.UtcNow
            };

            // Persistência
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.UserProfiles.AddAsync(userProfile, cancellationToken);
            await _context.UserSecurity.AddAsync(userSecurity, cancellationToken);
            await _context.UserRoles.AddAsync(userRole, cancellationToken);
            await _context.UserPreferences.AddAsync(userPreference, cancellationToken);
            await _context.UserLevels.AddAsync(userLevel, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            // Token
            var token = _jwtGenerator.GenerateToken(user.Id, user.Email, "user");
            var expiresAt = DateTime.UtcNow.AddMinutes(60);

            return new AuthResponseDto
            {
                Success = true,
                Message = "Usuário registrado com sucesso!",
                UserId = user.Id,
                Email = user.Email,
                FullName = userProfile.FullName,
                Token = token,
                ExpiresAt = expiresAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar usuário");

            return new AuthResponseDto
            {
                Success = false,
                Message = "Erro ao registrar usuário",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}