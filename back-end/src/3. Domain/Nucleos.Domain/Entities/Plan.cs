using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Plan
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int? MaxNucleos { get; set; }
    public decimal Price { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}