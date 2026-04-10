using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Blocos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Blocos.Queries;

public class GetBlocoByIdQuery : IRequest<BlocoDto>
{
    public Guid Id { get; set; }
}

public class GetBlocoByIdQueryHandler : IRequestHandler<GetBlocoByIdQuery, BlocoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetBlocoByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<BlocoDto> Handle(GetBlocoByIdQuery request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, cancellationToken);

        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.Id);

        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == bloco.NucleoId && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), bloco.NucleoId);

        return new BlocoDto
        {
            Id = bloco.Id,
            NucleoId = bloco.NucleoId,
            Tipo = bloco.Tipo,
            Titulo = bloco.Titulo ?? string.Empty,
            Posicao = bloco.Posicao,
            Configuracoes = bloco.Configuracoes != null ? bloco.Configuracoes.ToString() : null,
            CreatedAt = bloco.CreatedAt
        };
    }
}