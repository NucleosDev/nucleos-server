namespace Nucleos.Application.Features.Blocos.DTOs;

public class BlocoListDto
{
    public Guid Id { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public int Posicao { get; set; }
}
