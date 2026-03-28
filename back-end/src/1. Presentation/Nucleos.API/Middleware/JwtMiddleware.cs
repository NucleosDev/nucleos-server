namespace Nucleos.API.Middleware;
// JWT já é tratado pelo AddAuthentication/AddJwtBearer no Program.cs
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    public JwtMiddleware(RequestDelegate next) => _next = next;
    public Task InvokeAsync(HttpContext context) => _next(context);
}
