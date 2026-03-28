using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calculos.DTOs;
using Nucleos.Application.Features.Calculos.Services;

namespace Nucleos.Infrastructure.Calculo;

public class CalculoEngine : ICalculoEngine
{
    private readonly IApplicationDbContext _context;
    public CalculoEngine(IApplicationDbContext context) => _context = context;

    public async Task<double> ExecutarAsync(Guid blocoId, string tipoOperacao, string campo, string? agruparPor = null, CancellationToken ct = default)
    {
        var listaIds = await _context.Listas.Where(l => l.BlocoId == blocoId && l.DeletedAt == null).Select(l => l.Id).ToListAsync(ct);
        var itens = await _context.ItensLista.Where(i => listaIds.Contains(i.ListaId) && i.DeletedAt == null).ToListAsync(ct);
        var valores = campo.ToLower() switch
        {
            "quantidade" => itens.Select(i => (double)i.Quantidade),
            "valor_unitario" => itens.Select(i => (double)(i.ValorUnitario ?? 0)),
            "valor_total" => itens.Select(i => (double)(i.ValorTotal ?? 0)),
            _ => itens.Select(_ => 1.0)
        };
        var lista = valores.ToList();
        if (!lista.Any()) return 0;
        return tipoOperacao.ToLower() switch
        {
            "soma" => lista.Sum(),
            "media" => lista.Average(),
            "contagem" => lista.Count,
            "max" => lista.Max(),
            "min" => lista.Min(),
            _ => 0
        };
    }

    public async Task<CalculoResultadoDto> Calcular(Guid configId, object dados, CancellationToken ct)
    {
        var config = await _context.BlocoCalculos.FirstOrDefaultAsync(c => c.Id == configId, ct);
        if (config == null)
            return new CalculoResultadoDto { Valor = 0, Detalhes = "Configuração de cálculo não encontrada." };

        var valor = await ExecutarAsync(config.BlocoId, config.TipoOperacao, config.Campo, config.AgruparPor, ct);
        return new CalculoResultadoDto
        {
            Valor = (decimal)valor,
            Detalhes = $"Operação {config.TipoOperacao} sobre {config.Campo}: {valor}"
        };
    }
}
