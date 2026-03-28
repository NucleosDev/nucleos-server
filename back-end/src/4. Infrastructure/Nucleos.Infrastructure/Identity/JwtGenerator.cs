// src/4. Infrastructure/Nucleos.Infrastructure/Identity/JwtGenerator.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.Identity;

public class JwtGenerator : IJwtGenerator
{
    private readonly IConfiguration _configuration;

    public JwtGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Guid userId, string email, string role)
    {
        try
        {
            // CORREÇÃO: Tentar obter do ambiente primeiro
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? 
                        _configuration["Jwt:Key"] ??
                        "Nucleos-Super-Secret-Key-Min-32-Characters-Long-For-JWT!";
            
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? 
                           _configuration["Jwt:Issuer"] ??
                           "https://localhost:5000";
            
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? 
                             _configuration["Jwt:Audience"] ??
                             "https://localhost:5000";
            
            var expiresInMinutesStr = Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? 
                                      _configuration["Jwt:ExpiresInMinutes"] ??
                                      "60";
            
            if (!int.TryParse(expiresInMinutesStr, out var expiresInMinutes))
            {
                expiresInMinutes = 60;
                Console.WriteLine($"⚠️ JWT_EXPIRES_MINUTES inválido: '{expiresInMinutesStr}', usando {expiresInMinutes} minutos");
            }

            Console.WriteLine($"🔑 Gerando token com chave de tamanho: {jwtKey.Length} caracteres");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey); // Usar UTF8 em vez de ASCII
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Erro ao gerar token JWT: {ex.Message}");
            throw;
        }
    }
}