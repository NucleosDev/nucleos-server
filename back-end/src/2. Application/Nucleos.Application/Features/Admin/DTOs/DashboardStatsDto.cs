namespace Nucleos.Application.Features.Admin.DTOs;

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int ActiveUsers { get; set; }
    public int TotalNucleos { get; set; }
    public int TotalTarefas { get; set; }
    public int TotalHabitos { get; set; }
    public DateTime GeneratedAt { get; set; }
}
