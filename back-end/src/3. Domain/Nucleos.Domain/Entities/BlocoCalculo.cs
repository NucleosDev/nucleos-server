using System;

namespace Nucleos.Domain.Entities;

public class BlocoCalculo
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string TipoOperacao { get; set; }
    public string Campo { get; set; } = "valor_total";
    public string AgruparPor { get; set; }
    public string Config { get; set; } = "{}";
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual Bloco Bloco { get; set; }
}