namespace Nucleos.Domain.Entities;

public class NucleoCompartilhamento
{
    public Guid Id { get; set; }
    public Guid NucleoId { get; set; }
    public Guid OwnerUserId { get; set; }
    public Guid SharedWithUserId { get; set; }
    public string PermissionLevel { get; set; } = "view";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual Nucleo Nucleo { get; set; }
    public virtual User OwnerUser { get; set; }
    public virtual User SharedWithUser { get; set; }
}
