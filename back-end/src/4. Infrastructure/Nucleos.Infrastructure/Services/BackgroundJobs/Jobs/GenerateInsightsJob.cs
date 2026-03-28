using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.Services.BackgroundJobs.Jobs;
public class GenerateInsightsJob { private readonly ILogger<GenerateInsightsJob> _l; public GenerateInsightsJob(ILogger<GenerateInsightsJob> l) => _l = l; public Task RunAsync(CancellationToken ct = default) { _l.LogInformation("GenerateInsightsJob"); return Task.CompletedTask; } }
