using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calculos.DTOs;
using Nucleos.Application.Features.Calculos.Services;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calculos.Commands;

public class ExecutarCalculoCommand : IRequest<CalculoResultadoDto>
{
    public Guid BlocoId { get; set; }
}

public class ExecutarCalculoCommandHandler : IRequestHandler<ExecutarCalculoCommand, CalculoResultadoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ICalculoEngine _calculoEngine;

    public ExecutarCalculoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, ICalculoEngine calculoEngine)
    {
        _context = context;
        _currentUser = currentUser;
        _calculoEngine = calculoEngine;
    }

    public async Task<CalculoResultadoDto> Handle(ExecutarCalculoCommand request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos
            .Include(b => b.Calculo)  // Navigation property: BlocoCalculo
            .FirstOrDefaultAsync(b => b.Id == request.BlocoId, cancellationToken);

        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.BlocoId);

        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        var calculo = bloco.Calculo;
        if (calculo == null)
            throw new InvalidOperationException("No calculation configuration found for this block.");

        // Coletar dados dos itens das listas dentro deste bloco
        var listas = await _context.Listas
            .Where(l => l.BlocoId == request.BlocoId && l.DeletedAt == null)
            .Include(l => l.Itens.Where(i => i.DeletedAt == null))
            .ToListAsync(cancellationToken);

        var valores = new List<object>();
        foreach (var lista in listas)
        {
            foreach (var item in lista.Itens)
            {
                object valor = calculo.Campo switch
                {
                    "valor_total" => item.ValorTotal,
                    "quantidade" => item.Quantidade,
                    _ => 0
                };
                valores.Add(valor);
            }
        }

        return await _calculoEngine.Calcular(calculo.Id, valores, cancellationToken);
    }
}