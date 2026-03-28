using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calculos.Commands;

public class ConfigurarCalculoCommand : IRequest<Unit>
{
    public Guid BlocoId { get; set; }
    public string TipoOperacao { get; set; }
    public string Campo { get; set; } = "valor_total";
    public string AgruparPor { get; set; }
    public string Config { get; set; }   // armazena como string JSON
}

public class ConfigurarCalculoCommandHandler : IRequestHandler<ConfigurarCalculoCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public ConfigurarCalculoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    // Verifica se o usuário tem permissão de escrita no núcleo
    private async Task<bool> HasWriteAccess(Guid nucleoId)
    {
        var compartilhamento = await _context.NucleoCompartilhamentos
            .FirstOrDefaultAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == _currentUser.UserId);
        return compartilhamento != null && compartilhamento.PermissionLevel == "edit";
    }

    public async Task<Unit> Handle(ConfigurarCalculoCommand request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos.FindAsync(request.BlocoId);
        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.BlocoId);

        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId && !await HasWriteAccess(nucleo.Id))
            throw new ForbiddenException();

        var calculo = await _context.BlocoCalculos.FirstOrDefaultAsync(c => c.BlocoId == request.BlocoId, cancellationToken);
        if (calculo == null)
        {
            calculo = new BlocoCalculo
            {
                Id = Guid.NewGuid(),
                BlocoId = request.BlocoId,
                TipoOperacao = request.TipoOperacao,
                Campo = request.Campo,
                AgruparPor = request.AgruparPor,
                Config = request.Config ?? "{}",   // string
                CreatedAt = DateTime.UtcNow
            };
            await _context.BlocoCalculos.AddAsync(calculo, cancellationToken);
        }
        else
        {
            calculo.TipoOperacao = request.TipoOperacao;
            calculo.Campo = request.Campo;
            calculo.AgruparPor = request.AgruparPor;
            calculo.Config = request.Config ?? "{}";
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}