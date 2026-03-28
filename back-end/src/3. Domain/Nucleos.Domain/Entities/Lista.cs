using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Lista
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Nome { get; set; }
    public string TipoLista { get; set; } = "generica";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // Navigation
    public virtual Bloco Bloco { get; set; }
    public virtual ICollection<ItemLista> Itens { get; set; } = new List<ItemLista>();  // ← ADICIONAR ESTA LINHA
    public virtual ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();  // ← ADICIONAR ESTA LINHA
}