namespace Nucleos.Application.Features.Gamificacao.DTOs;

public class ConquistaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? IconeUrl { get; set; }
    public DateTime DesbloqueadoEm { get; set; }
}
