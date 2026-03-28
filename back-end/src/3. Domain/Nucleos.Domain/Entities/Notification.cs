using System;

namespace Nucleos.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Titulo { get; set; }
    public string Mensagem { get; set; }
    public bool Read { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual User User { get; set; }
}