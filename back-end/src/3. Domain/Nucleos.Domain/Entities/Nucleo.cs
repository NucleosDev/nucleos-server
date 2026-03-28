using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Nucleo
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? IconId { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; } = "pessoal";
    public string CorDestaque { get; set; }
    public string ImagemCapa { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    // ========== NAVEGAÇÕES ==========
    public virtual User User { get; set; }
    public virtual NucleoIcon Icon { get; set; }
    
    public virtual ICollection<Bloco> Blocos { get; set; } = new List<Bloco>();
    public virtual ICollection<CalendarioEvento> CalendarioEventos { get; set; } = new List<CalendarioEvento>();
    public virtual ICollection<NucleoTimer> NucleoTimers { get; set; } = new List<NucleoTimer>();
    public virtual ICollection<Meta> Metas { get; set; } = new List<Meta>();
    
    public virtual ICollection<NucleoCompartilhamento> Compartilhamentos { get; set; } = new List<NucleoCompartilhamento>();
    public virtual ICollection<NucleoRelation> Relations { get; set; } = new List<NucleoRelation>();
    public virtual ICollection<NucleoAchievement> Achievements { get; set; } = new List<NucleoAchievement>();
}