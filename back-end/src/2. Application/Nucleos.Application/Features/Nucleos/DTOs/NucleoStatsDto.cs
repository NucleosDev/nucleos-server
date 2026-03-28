namespace Nucleos.Application.Features.Nucleos.DTOs;

public class NucleoStatsDto
{
    public Guid NucleoId { get; set; }
    public int TotalBlocos { get; set; }
    public int TotalTarefas { get; set; }
    public int TarefasConcluidas { get; set; }
    public int TotalHabitos { get; set; }
    public int TotalXP { get; set; }
}
