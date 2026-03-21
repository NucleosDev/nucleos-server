namespace Nucleos.Domain.Entities;

public class NucleoRelation
{
    public Guid Id { get;     public Guid Id { get;     ucleoId { get; set; }
    public Guid TargetNucleoId { get; set; }
    public string RelationType { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Nucleo SourceNucleo { get; set; }
    public virtual Nucleo TargetNucleo { get; set; }
}
