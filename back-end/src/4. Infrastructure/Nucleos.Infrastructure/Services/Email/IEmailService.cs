namespace Nucleos.Infrastructure.Services.Email;
public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
}
