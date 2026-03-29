using System.Net;
using System.Text.Json;
using Nucleos.Application.Common.Exceptions;

namespace Nucleos.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    { _next = next; _logger = logger; }

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex) { await HandleExceptionAsync(context, ex); }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
        var (status, message) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message),
            UnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message),
            ForbiddenException => (HttpStatusCode.Forbidden, ex.Message),
            BusinessRuleException => (HttpStatusCode.UnprocessableEntity, ex.Message),
            Application.Common.Exceptions.ValidationException ve => (HttpStatusCode.BadRequest, "Erros de validação"),
            _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor")
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        var body = JsonSerializer.Serialize(new { status = (int)status, message, errors = ex is Application.Common.Exceptions.ValidationException ve2 ? (object)ve2.Errors : null });
        await context.Response.WriteAsync(body);
        
        _logger.LogError(ex, "Erro não tratado: {Message} | Tipo: {Type}", ex.Message, ex.GetType().FullName);
    }
}
