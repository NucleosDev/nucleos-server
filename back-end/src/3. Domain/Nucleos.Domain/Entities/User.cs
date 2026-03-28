using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Cpf { get; set; }
    public string PasswordHash { get; set; }
    public bool EmailVerified { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Navegações
    public virtual UserProfile Profile { get; set; }
    public virtual UserSecurity Security { get; set; }
    public virtual UserPreference Preference { get; set; }
    public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    public virtual ICollection<Nucleo> Nucleos { get; set; } = new List<Nucleo>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual ICollection<Streak> Streaks { get; set; } = new List<Streak>();
    public virtual UserLevel Level { get; set; }
    public virtual ICollection<UserConquista> Conquistas { get; set; } = new List<UserConquista>();
    public virtual ICollection<XP_Log> XpLogs { get; set; } = new List<XP_Log>();
    public virtual ICollection<EnergyLog> EnergyLogs { get; set; } = new List<EnergyLog>();
    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    public virtual ICollection<AIInteraction> AiInteractions { get; set; } = new List<AIInteraction>();
    public virtual AIContext AiContext { get; set; }
    public virtual Subscription Subscription { get; set; }
}