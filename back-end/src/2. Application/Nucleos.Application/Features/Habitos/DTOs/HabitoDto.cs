namespace Nucleos.Application.Features.Habitos.DTOs;

public class HabitoDto
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Frequencia { get; set; } = string.Empty;
    public List<int>? DiasSemana { get; set; }
    public int? MetaVezes { get; set; }
    public DateTime CreatedAt { get; set; }
}