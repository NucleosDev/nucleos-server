using System;

namespace Nucleos.Domain.Entities;

public class ActivityLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string Acao { get; set; }
    public string Metadata { get; set; } // JSON
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
}