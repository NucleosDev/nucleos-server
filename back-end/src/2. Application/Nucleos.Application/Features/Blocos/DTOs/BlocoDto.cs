namespace Nucleos.Application.Features.Blocos.DTOs;

public class BlocoDto
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int Posicao { get; set; }
    public string? Configuracoes { get; set; }
    public DateTime CreatedAt { get; set; }
}