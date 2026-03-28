namespace Nucleos.Application.Common.Interfaces;

public interface IQueueService
{
    Task PublishAsync<T>(string queue, T message, CancellationToken ct = default);
}
