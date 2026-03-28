using System;

namespace Nucleos.Domain.Entities;

public class UserConquista
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ConquistaId { get; set; }
    public DateTime DesbloqueadoEm { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
    public virtual Conquista Conquista { get; set; }
}