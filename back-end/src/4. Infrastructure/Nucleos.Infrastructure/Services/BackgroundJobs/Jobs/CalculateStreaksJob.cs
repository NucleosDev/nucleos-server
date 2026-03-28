using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.Services.BackgroundJobs.Jobs;
public class CalculateStreaksJob { private readonly ILogger<CalculateStreaksJob> _l; public CalculateStreaksJob(ILogger<CalculateStreaksJob> l) => _l = l; public Task RunAsync(CancellationToken ct = default) { _l.LogInformation("CalculateStreaksJob"); return Task.CompletedTask; } }
