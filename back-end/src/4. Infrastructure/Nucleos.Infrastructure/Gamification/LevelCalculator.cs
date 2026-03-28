namespace Nucleos.Infrastructure.Gamification;

public class LevelCalculator
{
    public (int level, long nextLevelXp) Calcular(long totalXp)
    {
        int level = 1;
        long threshold = 100;
        long accumulated = 0;
        while (accumulated + threshold <= totalXp)
        {
            accumulated += threshold;
            threshold = (long)(threshold * 1.5);
            level++;
        }
        return (level, threshold);
    }
}
