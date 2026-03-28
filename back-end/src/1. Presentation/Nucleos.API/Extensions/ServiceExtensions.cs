namespace Nucleos.API.Extensions;
public static class ServiceExtensions
{
    public static IApplicationBuilder UseNucleosMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<Middleware.ExceptionMiddleware>();
        app.UseMiddleware<Middleware.RequestLoggingMiddleware>();
        return app;
    }
}
