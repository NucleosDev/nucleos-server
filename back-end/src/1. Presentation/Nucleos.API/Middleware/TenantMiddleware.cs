namespace Nucleos.API.Middleware;
public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    public TenantMiddleware(RequestDelegate next) => _next = next;
    public Task InvokeAsync(HttpContext context) => _next(context);
}
