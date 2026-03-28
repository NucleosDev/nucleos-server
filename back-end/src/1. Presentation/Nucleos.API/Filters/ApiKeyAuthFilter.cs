using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Nucleos.API.Filters;
public class ApiKeyAuthFilter : IAuthorizationFilter
{
    private const string ApiKeyHeader = "X-Api-Key";
    private readonly IConfiguration _config;
    public ApiKeyAuthFilter(IConfiguration config) => _config = config;
    public void OnAuthorization(AuthorizationFilterContext ctx)
    {
        if (!ctx.HttpContext.Request.Headers.TryGetValue(ApiKeyHeader, out var key) || key != _config["ApiKey"])
            ctx.Result = new UnauthorizedResult();
    }
}
