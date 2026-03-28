using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
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
    public GetBlocoByIdQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<BlocoDto> Handle(GetBlocoByIdQuery request, CancellationToken ct)
    {
        var bloco = await _context.Blocos
                        .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Bloco), request.Id);

        return new BlocoDto
        {
            Id = bloco.Id,
            NucleoId = bloco.NucleoId,
            Tipo = bloco.Tipo,
            Titulo = bloco.Titulo,
            Posicao = bloco.Posicao,
            Configuracoes = bloco.Configuracoes,
            CreatedAt = bloco.CreatedAt
        };
    }
}