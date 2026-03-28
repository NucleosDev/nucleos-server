using Microsoft.AspNetCore.Mvc.Filters;
namespace Nucleos.API.Filters;
public class PermissionFilter : IAuthorizationFilter
{
    private readonly string _permission;
    public PermissionFilter(string permission) => _permission = permission;
    public void OnAuthorization(AuthorizationFilterContext context) { }
}
