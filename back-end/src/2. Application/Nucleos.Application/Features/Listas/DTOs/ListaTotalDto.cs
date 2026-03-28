namespace Nucleos.Application.Features.Listas.DTOs;

public class ListaTotalDto
{
    public Guid ListaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public int QuantidadeItens { get; set; }
}