namespace Nucleos.Application.Features.Auth.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
