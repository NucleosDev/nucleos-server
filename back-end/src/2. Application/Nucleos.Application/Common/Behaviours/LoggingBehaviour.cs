using MediatR;
using Microsoft.Extensions.Logging;

namespace Nucleos.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation("Nucleos Request: {Name} {@Request}", name, request);
        var response = await next();
        _logger.LogInformation("Nucleos Response: {Name} {@Response}", name, response);
        return response;
    }
}
