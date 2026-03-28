namespace Nucleos.Application.Features.Calculos.DTOs;

public class CalculoConfigDto
{
    public Guid BlocoId { get; set; }
    public string TipoOperacao { get; set; } = string.Empty;
    public string Campo { get; set; } = string.Empty;
    public string? AgruparPor { get; set; }
}
