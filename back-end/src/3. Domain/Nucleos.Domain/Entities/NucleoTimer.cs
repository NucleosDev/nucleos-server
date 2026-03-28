using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nucleos.Domain.Entities;

[Table("timers")] // Nome da tabela real no banco
public class NucleoTimer
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string? Titulo { get; set; }
    public DateTime? Inicio { get; set; }
    public DateTime? Fim { get; set; }
    public int? DuracaoSegundos { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual Nucleo? Nucleo { get; set; }
}