using System;

namespace Nucleos.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullName { get; set; }
    public string Nickname { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public virtual User User { get; set; }
}