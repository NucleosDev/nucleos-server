using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Categoria
{
    public Guid Id { get; set; }
    public Guid ListaId { get; set; }
    public string Nome { get; set; }
    public string Cor { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual Lista Lista { get; set; }
    public virtual ICollection<ItemLista> Itens { get; set; } = new List<ItemLista>();  // ← ADICIONAR ESTA LINHA
}