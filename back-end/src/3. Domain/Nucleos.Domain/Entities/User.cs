using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Cpf { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    public bool EmailVerified { get; set; }
    public bool Active { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navegações
    public virtual UserProfile? Profile { get; private set; }
    public virtual UserSecurity? Security { get; private set; }
    public virtual UserPreference? Preference { get; private set; }

    public virtual ICollection<UserRole> Roles { get; private set; } = new List<UserRole>();
    public virtual ICollection<Nucleo> Nucleos { get; private set; } = new List<Nucleo>();
    public virtual ICollection<Notification> Notifications { get; private set; } = new List<Notification>();
    public virtual ICollection<Streak> Streaks { get; private set; } = new List<Streak>();

    public virtual UserLevel? Level { get; private set; }

    public virtual ICollection<UserConquista> Conquistas { get; private set; } = new List<UserConquista>();
    public virtual ICollection<XP_Log> XpLogs { get; private set; } = new List<XP_Log>();
    public virtual ICollection<EnergyLog> EnergyLogs { get; private set; } = new List<EnergyLog>();
    public virtual ICollection<ActivityLog> ActivityLogs { get; private set; } = new List<ActivityLog>();
    public virtual ICollection<AIInteraction> AiInteractions { get; private set; } = new List<AIInteraction>();

    public virtual AIContext? AiContext { get; private set; }
    public virtual Subscription? Subscription { get; private set; }

    // Construtor para EF
    protected User() { }

    // Construtor principal
    public User(string email, string phone, string cpf, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        Phone = phone;
        Cpf = cpf;
        PasswordHash = passwordHash;
        EmailVerified = false;
        Active = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // Métodos de domínio
    public void VerifyEmail()
    {
        EmailVerified = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Active = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        Active = false;
    }

    public void UpdateProfile(string email, string phone)
    {
        Email = email;
        Phone = phone;
        UpdatedAt = DateTime.UtcNow;
    }
}