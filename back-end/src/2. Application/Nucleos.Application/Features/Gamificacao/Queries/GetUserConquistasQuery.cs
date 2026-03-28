using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Gamificacao.DTOs;

namespace Nucleos.Application.Features.Gamificacao.Queries;

public class GetUserConquistasQuery : IRequest<List<ConquistaDto>> { public Guid UserId { get; set; } }

public class GetUserConquistasQueryHandler : IRequestHandler<GetUserConquistasQuery, List<ConquistaDto>>
{
    private readonly IApplicationDbContext _context;
    public GetUserConquistasQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<ConquistaDto>> Handle(GetUserConquistasQuery request, CancellationToken ct)
        => await _context.UserConquistas
            .Include(uc => uc.Conquista)
            .Where(uc => uc.UserId == request.UserId)
            .Select(uc => new ConquistaDto { Id = uc.ConquistaId, Nome = uc.Conquista.Nome, Descricao = uc.Conquista.Descricao, IconeUrl = uc.Conquista.IconeUrl, DesbloqueadoEm = uc.DesbloqueadoEm })
            .ToListAsync(ct);
}
