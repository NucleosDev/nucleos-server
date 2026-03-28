using System;

namespace Nucleos.Domain.Entities;

public class XP_Log
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid? NucleoId { get; set; }
    public int XpAmount { get; set; }
    public string Source { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
    public virtual Nucleo Nucleo { get; set; }
}