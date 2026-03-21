namespace Nucleos.Domain.Entities;

public class NucleoIcon
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string IconUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual ICollection<Nucleo> Nucleos { get; set; } = new List<Nucleo>();
}
