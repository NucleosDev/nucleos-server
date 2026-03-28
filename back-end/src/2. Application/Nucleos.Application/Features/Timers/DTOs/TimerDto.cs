namespace Nucleos.Application.Features.Timers.DTOs;

public class TimerDto
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string? Titulo { get; set; }
    public DateTime? Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public int? DuracaoSegundos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
