namespace Nucleos.Application.Features.Admin.DTOs;

public class PlanDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public int DuracaoDias { get; set; }
    // Adicione outras propriedades conforme necessário
}
