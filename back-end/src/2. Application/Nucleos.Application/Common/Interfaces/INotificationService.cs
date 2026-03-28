namespace Nucleos.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendAsync(Guid userId, string titulo, string mensagem, CancellationToken ct = default);
}
