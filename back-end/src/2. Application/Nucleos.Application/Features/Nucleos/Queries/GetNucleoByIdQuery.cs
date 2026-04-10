using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Nucleos.DTOs;
using Nucleos.Application.Features.Blocos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Nucleos.Queries;

public class GetNucleoByIdQuery : IRequest<NucleoDetailDto>
{
    public Guid Id { get; set; }
}

public class GetNucleoByIdQueryHandler : IRequestHandler<GetNucleoByIdQuery, NucleoDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetNucleoByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<NucleoDetailDto> Handle(GetNucleoByIdQuery request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.Id);

        var blocos = await _context.Blocos
            .Where(b => b.NucleoId == request.Id && b.DeletedAt == null)
            .OrderBy(b => b.Posicao)
            .Select(b => new BlocoDto
            {
                Id = b.Id,
                NucleoId = b.NucleoId,
                Tipo = b.Tipo,
                Titulo = b.Titulo ?? string.Empty,
                Posicao = b.Posicao,
                Configuracoes = b.Configuracoes != null ? b.Configuracoes.ToString() : null,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new NucleoDetailDto
        {
            Id = nucleo.Id,
            Nome = nucleo.Nome,
            Descricao = nucleo.Descricao,
            Tipo = nucleo.Tipo,
            CorDestaque = nucleo.CorDestaque,
            ImagemCapa = nucleo.ImagemCapa,
            CreatedAt = nucleo.CreatedAt,
            UpdatedAt = nucleo.UpdatedAt,
            Blocos = blocos
        };
    }
}