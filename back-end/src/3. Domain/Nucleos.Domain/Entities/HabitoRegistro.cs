using System;

namespace Nucleos.Domain.Entities;

public class HabitoRegistro
{
    public Guid Id { get; set; }
    public Guid HabitoId { get; set; }
    public DateTime Data { get; set; }
    public int VezesCompletadas { get; set; } = 1;
    public DateTime CreatedAt { get; set; }
    
    public virtual Habito Habito { get; set; }
}
