using System.ComponentModel.DataAnnotations;

namespace Nucleos.Application.Features.Auth.DTOs;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "Refresh token é obrigatório")]
    public string RefreshToken { get; set; } = string.Empty;
}