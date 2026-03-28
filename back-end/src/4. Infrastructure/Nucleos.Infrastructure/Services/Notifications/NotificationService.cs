using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Services.Notifications;

public class NotificationService : INotificationService, Application.Common.Interfaces.INotificationService
{
    private readonly IApplicationDbContext _context;
    public NotificationService(IApplicationDbContext context) => _context = context;

    public async Task SendAsync(Guid userId, string titulo, string mensagem, CancellationToken ct = default)
    {
        var notification = new Notification { Id = Guid.NewGuid(), UserId = userId, Titulo = titulo, Mensagem = mensagem, Read = false, CreatedAt = global::System.DateTime.UtcNow };
        await _context.Notifications.AddAsync(notification, ct);
        await _context.SaveChangesAsync(ct);
    }
}
