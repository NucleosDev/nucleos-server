using System;

namespace Nucleos.Domain.Entities;

public class Meta
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; }
    public decimal ValorMeta { get; set; }
    public decimal ValorAtual { get; set; } = 0;
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation
    public virtual Nucleo Nucleo { get; set; }
}