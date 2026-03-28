using System;

namespace Nucleos.Domain.Entities;

public class UserPreference
{
    public Guid UserId { get; set; }
    public string Theme { get; set; } = "system";
    public string Language { get; set; } = "pt-BR";
    public string Notifications { get; set; } = "{\"push\": true, \"email\": true, \"streaks\": true}";
    public string Shortcuts { get; set; } = "{}";
    public string DashboardLayout { get; set; } = "{}";
    public DateTime UpdatedAt { get; set; }
    
    public virtual User User { get; set; }
}