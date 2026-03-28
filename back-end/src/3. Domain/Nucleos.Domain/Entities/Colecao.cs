using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Colecao
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Nome { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual Bloco Bloco { get; set; }
    public virtual ICollection<Campo> Campos { get; set; } = new List<Campo>();
    public virtual ICollection<Item> Itens { get; set; } = new List<Item>();
}