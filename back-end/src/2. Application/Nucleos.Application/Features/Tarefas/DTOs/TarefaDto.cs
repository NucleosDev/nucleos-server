namespace Nucleos.Application.Features.Tarefas.DTOs;

public class TarefaDto
{
    public Guid Id { get; set; }
    public Guid BlocoId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Prioridade { get; set; }
    public string? Status { get; set; }
    public DateTime? DataVencimento { get; set; }
    public DateTime? ConcluidaEm { get; set; }
    public int Posicao { get; set; }
    public DateTime CreatedAt { get; set; }
}