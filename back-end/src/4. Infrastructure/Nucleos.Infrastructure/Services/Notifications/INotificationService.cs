namespace Nucleos.Infrastructure.Services.Notifications;
public interface INotificationService
{
    Task SendAsync(Guid userId, string titulo, string mensagem, CancellationToken ct = default);
}
