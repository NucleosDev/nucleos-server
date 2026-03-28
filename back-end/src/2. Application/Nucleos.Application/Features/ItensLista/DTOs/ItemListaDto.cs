namespace Nucleos.Application.Features.ItensLista.DTOs;

public class ItemListaDto
{
    public Guid Id { get; set; }
    public Guid ListaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal? ValorUnitario { get; set; }
    public decimal? ValorTotal { get; set; }
    public bool Checked { get; set; }
    public Guid? CategoriaId { get; set; }
    public DateTime CreatedAt { get; set; }
}