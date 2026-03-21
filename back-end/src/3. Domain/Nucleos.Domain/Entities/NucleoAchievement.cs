namespace Nucleos.Domain.Entities;

public class NucleoAchievement
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string AchievementType { get; set; }
    public int CurrentValue { get; set; } = 0;
    public int TargetValue { get; set; }
    public DateTime? UnlockedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Nucleo Nucleo { get; set; }
}
