using System.ComponentModel.DataAnnotations;

namespace Nucleos.Application.Features.Auth.DTOs;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MinLength(3, ErrorMessage = "Nome deve ter no mínimo 3 caracteres")]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [MinLength(8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    [MinLength(11, ErrorMessage = "CPF deve ter 11 caracteres")]
    [MaxLength(14, ErrorMessage = "CPF inválido")]
    public string Cpf { get; set; } = string.Empty; // ← obrigatório
}