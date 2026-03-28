using System;

namespace Nucleos.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PlanId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
    public virtual Plan Plan { get; set; }
}