using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.Services.BackgroundJobs;
public class HangfireService : IJobService
{
    private readonly ILogger<HangfireService> _l;
    public HangfireService(ILogger<HangfireService> l) => _l = l;
    public void Enqueue(string job, object? args = null) => _l.LogInformation("Enqueue: {Job}", job);
    public void Schedule(string job, TimeSpan delay, object? args = null) => _l.LogInformation("Schedule: {Job}", job);
}
