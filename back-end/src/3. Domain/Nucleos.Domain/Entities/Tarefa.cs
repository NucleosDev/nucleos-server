using System;

namespace Nucleos.Domain.Entities;

public class Tarefa
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string Prioridade { get; set; } = "media";
    public string Status { get; set; } = "pendente";
    public DateTime? DataVencimento { get; set; }
    public DateTime? ConcluidaEm { get; set; }
    public int Posicao { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public virtual Bloco Bloco { get; set; }
}