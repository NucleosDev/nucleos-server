using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.Identity;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtGenerator> _logger;

    public JwtGenerator(
        IConfiguration configuration,
        ILogger<JwtGenerator> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string GenerateToken(Guid userId, string email, string role)
    {
        try
        {
            var jwtKey = _configuration["JWT_KEY"];
var jwtIssuer = _configuration["JWT_ISSUER"] ?? "https://localhost:5000";
var jwtAudience = _configuration["JWT_AUDIENCE"] ?? "https://localhost:5000";
var expiresInMinutesStr = _configuration["JWT_EXPIRES_MINUTES"];

            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT_KEY não configurada");

            if (jwtKey.Length < 32)
                throw new Exception("JWT_KEY deve ter no mínimo 32 caracteres");

            if (!int.TryParse(expiresInMinutesStr, out var expiresInMinutes))
                expiresInMinutes = 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(expiresInMinutes),
                NotBefore = now,
                IssuedAt = now,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            _logger.LogInformation("JWT gerado para usuário {UserId}", userId);
			_logger.LogInformation("JWT_KEY (Generator): {Key}", jwtKey);

            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar token JWT");
            throw;
        }
    }
}