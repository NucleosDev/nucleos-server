namespace Nucleos.Application.Features.Auth.DTOs;

public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string? RefreshToken { get; set; } // ← TORNAR OPCIONAL
    public DateTime ExpiresAt { get; set; }
    public List<string>? Errors { get; set; }
    public string Cpf { get; set; } = string.Empty;
}