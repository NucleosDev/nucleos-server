using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Auth.DTOs;
using Nucleos.Domain.Entities;
using BCrypt.Net;

namespace Nucleos.Application.Features.Auth.Commands;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty; // ← ADICIONADO
    public string? Phone { get; set; }
    public string Cpf { get; set; } = string.Empty; // ← OBRIGATÓRIO
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
            // Validação de senhas
            if (request.Password != request.ConfirmPassword)
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "As senhas não conferem",
                    Errors = new List<string> { "As senhas não conferem" }
                };

            // Validação de CPF
            if (string.IsNullOrWhiteSpace(request.Cpf))
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "CPF é obrigatório",
                    Errors = new List<string> { "CPF é obrigatório" }
                };

            // Limpar CPF
            var cleanCpf = request.Cpf.Replace(".", "").Replace("-", "").Replace("/", "");

            // Validar formato do CPF
            if (!IsValidCpf(cleanCpf))
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "CPF inválido",
                    Errors = new List<string> { "CPF inválido" }
                };

            // Verificar se email já existe
            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "E-mail já cadastrado",
                    Errors = new List<string> { "E-mail já cadastrado" }
                };

            // Verificar se CPF já existe
            var cpfExists = await _context.Users.AnyAsync(u => u.Cpf == cleanCpf, cancellationToken);
            if (cpfExists)
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "CPF já cadastrado",
                    Errors = new List<string> { "CPF já cadastrado" }
                };

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Phone = request.Phone,
                Cpf = cleanCpf, // ← OBRIGATÓRIO
                PasswordHash = passwordHash,
                Active = true,
                EmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var nickname = string.IsNullOrEmpty(request.Nickname)
                ? request.FullName.Split(' ')[0]
                : request.Nickname;

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                FullName = request.FullName,
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

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.UserProfiles.AddAsync(userProfile, cancellationToken);
            await _context.UserSecurity.AddAsync(userSecurity, cancellationToken);
            await _context.UserRoles.AddAsync(userRole, cancellationToken);
            await _context.UserPreferences.AddAsync(userPreference, cancellationToken);
            await _context.UserLevels.AddAsync(userLevel, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

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

    // Método para validar CPF
    private bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        if (cpf.Length != 11)
            return false;

        // Verificar se todos os dígitos são iguais
        if (cpf.Distinct().Count() == 1)
            return false;

        // Calcular primeiro dígito verificador
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += int.Parse(cpf[i].ToString()) * (10 - i);

        int firstDigit = 11 - (sum % 11);
        if (firstDigit >= 10) firstDigit = 0;

        if (firstDigit != int.Parse(cpf[9].ToString()))
            return false;

        // Calcular segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += int.Parse(cpf[i].ToString()) * (11 - i);

        int secondDigit = 11 - (sum % 11);
        if (secondDigit >= 10) secondDigit = 0;

        return secondDigit == int.Parse(cpf[10].ToString());
    }
}