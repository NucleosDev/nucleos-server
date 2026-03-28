namespace Nucleos.Application.Features.Habitos.DTOs;

public class HabitoProgressoDto
{
    public Guid HabitoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalCompletados { get; set; }
    public double MediaDiaria { get; set; }
    public List<RegistroDto> Registros { get; set; } = new();
}

public class RegistroDto
{
    public DateTime Data { get; set; }
    public int Vezes { get; set; }
}