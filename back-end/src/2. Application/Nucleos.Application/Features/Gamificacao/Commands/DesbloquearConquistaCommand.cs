using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Gamificacao.Commands;

public class DesbloquearConquistaCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid ConquistaId { get; set; }
}

public class DesbloquearConquistaCommandHandler : IRequestHandler<DesbloquearConquistaCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public DesbloquearConquistaCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<bool> Handle(DesbloquearConquistaCommand request, CancellationToken ct)
    {
        var ja = await _context.UserConquistas.AnyAsync(u => u.UserId == request.UserId && u.ConquistaId == request.ConquistaId, ct);
        if (ja) return false;
        await _context.UserConquistas.AddAsync(new UserConquista { Id = Guid.NewGuid(), UserId = request.UserId, ConquistaId = request.ConquistaId, DesbloqueadoEm = DateTime.UtcNow }, ct);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
