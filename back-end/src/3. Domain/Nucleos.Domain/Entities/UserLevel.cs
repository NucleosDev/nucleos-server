using System;

namespace Nucleos.Domain.Entities;

public class UserLevel
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public int Level { get; set; } = 1;
    public int CurrentXp { get; set; } = 0;
    public int NextLevelXp { get; set; } = 100;
    public int TotalXpEarned { get; set; } = 0;
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
}