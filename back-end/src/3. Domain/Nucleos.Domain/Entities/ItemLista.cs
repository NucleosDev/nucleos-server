using System;

namespace Nucleos.Domain.Entities;

public class ItemLista
{
    public Guid Id { get; set; }
    public Guid ListaId { get; set; }
    public Guid? CategoriaId { get; set; }
    public string Nome { get; set; }
    public decimal Quantidade { get; set; } = 1;
    public decimal? ValorUnitario { get; set; }
    public bool Checked { get; set; } = false;
    public decimal? ValorTotal { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Navigation
    public virtual Lista Lista { get; set; }
    public virtual Categoria Categoria { get; set; }  // ← ADICIONAR ESTA LINHA
}