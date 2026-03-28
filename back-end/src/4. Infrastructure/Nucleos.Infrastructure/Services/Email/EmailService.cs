using Microsoft.Extensions.Logging;

namespace Nucleos.Infrastructure.Services.Email;

public class EmailService : IEmailService, Application.Common.Interfaces.IEmailService
{
    private readonly ILogger<EmailService> _logger;
    public EmailService(ILogger<EmailService> logger) => _logger = logger;

    public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // TODO: integrar com SMTP / SendGrid / Resend
        _logger.LogInformation("Email para {To}: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}
