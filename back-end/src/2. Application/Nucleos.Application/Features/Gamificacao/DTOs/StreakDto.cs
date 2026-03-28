namespace Nucleos.Application.Features.Gamificacao.DTOs;

public class StreakDto
{
    public Guid Id { get; set; }
    public Guid? NucleoId { get; set; }
    public string StreakType { get; set; } = string.Empty;
    public int CurrentStreak { get; set; }
    public int MaxStreak { get; set; }
    public DateTime? LastActivityDate { get; set; }
}
