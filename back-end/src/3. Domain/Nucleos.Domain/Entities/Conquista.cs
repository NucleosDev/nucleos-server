using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Conquista
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string IconeUrl { get; set; }
    public string Tipo { get; set; }
    public string Condicao { get; set; } // JSON
    public int XpRecompensa { get; set; } = 100;
    public DateTime CreatedAt { get; set; }
    
    // Navigation
    public virtual ICollection<UserConquista> UserConquistas { get; set; } = new List<UserConquista>();
}