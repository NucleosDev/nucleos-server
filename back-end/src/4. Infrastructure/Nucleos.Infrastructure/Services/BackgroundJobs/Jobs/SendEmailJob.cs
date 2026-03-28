using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.Services.BackgroundJobs.Jobs;
public class SendEmailJob { private readonly ILogger<SendEmailJob> _l; public SendEmailJob(ILogger<SendEmailJob> l) => _l = l; public Task RunAsync(string to, string subject, string body, CancellationToken ct = default) { _l.LogInformation("SendEmailJob to {To}", to); return Task.CompletedTask; } }
