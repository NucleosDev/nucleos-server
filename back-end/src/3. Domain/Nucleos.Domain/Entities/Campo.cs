using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Campo
{
    public Guid Id { get; set; }
    public Guid ColecaoId { get; set; }
    public string Nome { get; set; }
    public string TipoCampo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual Colecao Colecao { get; set; }
    public virtual ICollection<ItemValor> Valores { get; set; } = new List<ItemValor>();
}