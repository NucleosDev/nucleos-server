namespace Nucleos.Domain.Entities;

public class AIContext
{
    public Guid Id { get; set; }
                                              c string LastSummary { get; set; }
    public string PreferredStyle { get; set; }
    public DateTime? LastInteraction { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User User { get; set; }
}
