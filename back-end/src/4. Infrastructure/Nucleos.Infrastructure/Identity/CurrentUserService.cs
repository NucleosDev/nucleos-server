using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
        => _httpContextAccessor = httpContextAccessor;

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
    
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    
    public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
    
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    // Implementação do método que estava faltando
    public bool HasPermission(string permission)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null) return false;

        // Verifica se o usuário tem uma claim de permissão com o valor solicitado
        return user.HasClaim(c => c.Type == "Permission" && c.Value == permission) ||
               user.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == permission);
    }
}