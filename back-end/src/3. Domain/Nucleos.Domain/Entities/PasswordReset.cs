namespace Nucleos.Domain.Entities;

public class PasswordReset
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool Used { get; set; } = false;
    public virtual User User { get; set; }
}
