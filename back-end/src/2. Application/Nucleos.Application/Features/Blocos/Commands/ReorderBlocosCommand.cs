using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Blocos.Commands;

public class ReorderBlocosCommand : IRequest<Unit>
{
    public Guid NucleoId { get; set; }
    public List<BlocoOrder> Orders { get; set; }
}

public class BlocoOrder
{
    public Guid Id { get; set; }
    public int Posicao { get; set; }
}

public class ReorderBlocosCommandHandler : IRequestHandler<ReorderBlocosCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ReorderBlocosCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Unit> Handle(ReorderBlocosCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == request.NucleoId && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        foreach (var order in request.Orders)
        {
            var bloco = await _context.Blocos
                .FirstOrDefaultAsync(b => b.Id == order.Id && b.NucleoId == request.NucleoId, cancellationToken);
            if (bloco != null)
            {
                bloco.Posicao = order.Posicao;
                bloco.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}