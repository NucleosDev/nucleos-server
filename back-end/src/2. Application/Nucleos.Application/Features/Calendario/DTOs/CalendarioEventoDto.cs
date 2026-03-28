namespace Nucleos.Application.Features.Calendario.DTOs;

public class CalendarioEventoDto
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime? DataEvento { get; set; }
    public int? DuracaoMinutos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
