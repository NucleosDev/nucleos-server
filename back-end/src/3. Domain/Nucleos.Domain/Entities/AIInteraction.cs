namespace Nucleos.Domain.Entities;

public class AIInteraction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Mensagem { get; set; }
    public string Resposta { get; set; }
    public string Contexto { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual User User { get; set; }
}
