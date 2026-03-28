namespace Nucleos.Application.Features.Listas.DTOs;

public class ListaDto
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? TipoLista { get; set; }
    public DateTime CreatedAt { get; set; }
}