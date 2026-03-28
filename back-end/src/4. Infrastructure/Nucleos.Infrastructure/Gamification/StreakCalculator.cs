namespace Nucleos.Infrastructure.Gamification;

public class StreakCalculator
{
    public bool EstaEmStreak(DateTime? lastActivity)
    {
        if (lastActivity == null) return false;
        return (DateTime.UtcNow.Date - lastActivity.Value.Date).TotalDays <= 1;
    }
}
