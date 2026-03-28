using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Admin.Commands;

public class ManageSubscriptionCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public int DurationDays { get; set; } = 30;
}

public class ManageSubscriptionCommandHandler : IRequestHandler<ManageSubscriptionCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public ManageSubscriptionCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(ManageSubscriptionCommand request, CancellationToken ct)
    {
        var sub = await _context.Subscriptions.FirstOrDefaultAsync(s => s.UserId == request.UserId, ct);
        if (sub == null)
        {
            sub = new Subscription { Id = Guid.NewGuid(), UserId = request.UserId, PlanId = request.PlanId, StartedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddDays(request.DurationDays) };
            await _context.Subscriptions.AddAsync(sub, ct);
        }
        else
        {
            sub.PlanId = request.PlanId;
            sub.ExpiresAt = DateTime.UtcNow.AddDays(request.DurationDays);
        }
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
