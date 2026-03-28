using Microsoft.Extensions.Logging;

namespace Nucleos.Infrastructure.Services.Queue;

public class RabbitMQService : IQueueService, Application.Common.Interfaces.IQueueService
{
    private readonly ILogger<RabbitMQService> _logger;
    public RabbitMQService(ILogger<RabbitMQService> logger) => _logger = logger;

    public Task PublishAsync<T>(string queue, T message, CancellationToken ct = default)
    {
        // TODO: conectar ao RabbitMQ
        _logger.LogInformation("Queue [{Queue}]: {Message}", queue, System.Text.Json.JsonSerializer.Serialize(message));
        return Task.CompletedTask;
    }
}
