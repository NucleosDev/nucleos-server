using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Blocos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Blocos.Commands;

public class UpdateBlocoCommand : IRequest<BlocoDto>
{
    public Guid Id { get; set; }
    public string? Tipo { get; set; }
    public string? Titulo { get; set; }
    public int? Posicao { get; set; }
    public string? Configuracoes { get; set; }
}

public class UpdateBlocoCommandHandler : IRequestHandler<UpdateBlocoCommand, BlocoDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateBlocoCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<BlocoDto> Handle(UpdateBlocoCommand request, CancellationToken ct)
    {
        var bloco = await _context.Blocos
                        .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedAt == null, ct)
                    ?? throw new NotFoundException(nameof(Bloco), request.Id);

        if (request.Tipo != null) bloco.Tipo = request.Tipo;
        if (request.Titulo != null) bloco.Titulo = request.Titulo;
        if (request.Posicao.HasValue) bloco.Posicao = request.Posicao.Value;
        if (request.Configuracoes != null) bloco.Configuracoes = request.Configuracoes;
        bloco.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

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