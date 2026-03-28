namespace Nucleos.Application.Common.Interfaces;

public interface IJwtGenerator
{
    string GenerateToken(Guid userId, string email, string role);
    // string GenerateRefreshToken();
}