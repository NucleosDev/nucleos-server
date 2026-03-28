using System;

namespace Nucleos.Domain.Entities;

public class Streak
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? NucleoId { get; set; }
    public string StreakType { get; set; }
    public int CurrentStreak { get; set; } = 0;
    public int MaxStreak { get; set; } = 0;
    public DateTime? LastActivityDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
    public virtual Nucleo Nucleo { get; set; }
}