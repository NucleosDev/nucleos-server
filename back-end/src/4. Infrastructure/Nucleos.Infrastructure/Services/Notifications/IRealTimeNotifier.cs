namespace Nucleos.Infrastructure.Services.Notifications;
public interface IRealTimeNotifier
{
    Task NotifyAsync(Guid userId, string eventType, object payload, CancellationToken ct = default);
}
