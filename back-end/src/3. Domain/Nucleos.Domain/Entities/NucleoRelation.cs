using System;

namespace Nucleos.Domain.Entities;

public class NucleoRelation
{
    public Guid Id { get; set; }
    public Guid SourceNucleoId { get; set; }
    public Guid TargetNucleoId { get; set; }
    public string RelationType { get; set; }
    public DateTime CreatedAt { get; set; }
}