using System;

namespace Nucleos.Domain.Entities;

public class ItemValor
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid CampoId { get; set; }
    public string ValorTexto { get; set; }
    public decimal? ValorNumerico { get; set; }
    public DateTime? ValorData { get; set; }
    public bool? ValorBooleano { get; set; }
    
    // Navigation
    public virtual Item Item { get; set; }
    public virtual Campo Campo { get; set; }
}