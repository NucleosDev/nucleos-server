using Microsoft.Extensions.Logging;
namespace Nucleos.Infrastructure.External;
public class StripeService
{
    private readonly ILogger<StripeService> _l;
    public StripeService(ILogger<StripeService> l) => _l = l;
    public Task<string> CreateCheckoutSessionAsync(Guid userId, string planId, CancellationToken ct = default)
    {
        _l.LogInformation("Stripe checkout {UserId} {PlanId}", userId, planId);
        return Task.FromResult("session_placeholder");
    }
}
