using System;

namespace Nucleos.Domain.Entities;

public class AIContext
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string LastSummary { get; set; }
    public string PreferredStyle { get; set; }
    public DateTime? LastInteraction { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }  // ← ADICIONAR ESTA LINHA
}