namespace Nucleos.Application.Features.IA.DTOs;

public class InsightDto
{
    public Guid Id { get; set; }
    public string InsightType { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool Applied { get; set; }
    public DateTime CreatedAt { get; set; }
}
