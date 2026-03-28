using System;

namespace Nucleos.Domain.Entities;

public class CalendarioEvento
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime? DataEvento { get; set; }
    public int? DuracaoMinutos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual Nucleo Nucleo { get; set; }
}