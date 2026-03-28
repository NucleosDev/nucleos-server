namespace Nucleos.Application.Features.IA.DTOs;

public class MensagemDto
{
    public string Role { get; set; } = "user"; // user | assistant
    public string Conteudo { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
