namespace Nucleos.Application.Features.Nucleos.DTOs;

public class NucleoListDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? CorDestaque { get; set; }
    public DateTime CreatedAt { get; set; }
}
