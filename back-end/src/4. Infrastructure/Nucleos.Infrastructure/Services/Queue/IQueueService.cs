namespace Nucleos.Infrastructure.Services.Queue;
public interface IQueueService
{
    Task PublishAsync<T>(string queue, T message, CancellationToken ct = default);
}
