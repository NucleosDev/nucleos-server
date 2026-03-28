using System;
using System.Collections.Generic;

namespace Nucleos.Domain.Entities;

public class Bloco : SoftDeleteEntity
{
    public Guid NucleoId { get; set; }
    public string Tipo { get; set; }
    public string Titulo { get; set; }
    public int Posicao { get; set; }
    public string Configuracoes { get; set; } = "{}";
    
    // Navigation
    public virtual Nucleo Nucleo { get; set; }
    public virtual ICollection<Lista> Listas { get; set; } = new List<Lista>();
    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
    public virtual ICollection<Habito> Habitos { get; set; } = new List<Habito>();
    public virtual ICollection<Colecao> Colecoes { get; set; } = new List<Colecao>();
    public virtual BlocoCalculo Calculo { get; set; }
}