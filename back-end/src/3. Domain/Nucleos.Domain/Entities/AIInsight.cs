namespace Nucleos.Domain.Entities;

public class AIInsight
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? NucleoId { get; set; }
    public string InsightType { get; set; }
    public string Mensagem { get; set; }
    public int Priority { get; set; } = 1;
    public bool Applied { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public virtual User User { get; set; }
    public virtual Nucleo Nucleo { get; set; }
}
