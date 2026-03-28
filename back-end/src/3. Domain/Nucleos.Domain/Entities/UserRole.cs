using System;

namespace Nucleos.Domain.Entities;

public class UserRole
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; } = "user";
    
    public virtual User User { get; set; }
}