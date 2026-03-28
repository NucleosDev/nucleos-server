using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Habito
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Nome { get; set; }
    public string Frequencia { get; set; }
    public int? MetaVezes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual Bloco Bloco { get; set; }
    public virtual ICollection<HabitoRegistro> Registros { get; set; } = new List<HabitoRegistro>();
}