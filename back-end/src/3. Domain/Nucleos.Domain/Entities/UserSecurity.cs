using System;

namespace Nucleos.Domain.Entities;

public class UserSecurity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime? LastLogin { get; set; }
    public int FailedAttempts { get; set; } = 0;
    public DateTime PasswordUpdatedAt { get; set; }
    
    public virtual User User { get; set; }
}