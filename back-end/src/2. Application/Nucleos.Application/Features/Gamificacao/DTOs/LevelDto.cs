namespace Nucleos.Application.Features.Gamificacao.DTOs;

public class LevelDto
{
    public Guid UserId { get; set; }
    public int Level { get; set; }
    public int CurrentXp { get; set; }
    public int NextLevelXp { get; set; }
    public int TotalXpEarned { get; set; }
    public double Progress => NextLevelXp > 0 ? (double)CurrentXp / NextLevelXp * 100 : 0;
}
