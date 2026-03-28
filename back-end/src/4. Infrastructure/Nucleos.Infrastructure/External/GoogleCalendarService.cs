using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.External;
public class GoogleCalendarService
{
    private readonly ILogger<GoogleCalendarService> _l;
    public GoogleCalendarService(ILogger<GoogleCalendarService> l) => _l = l;
    public Task SyncAsync(Guid userId, CancellationToken ct = default) { _l.LogInformation("GCal sync {UserId}", userId); return Task.CompletedTask; }
}
