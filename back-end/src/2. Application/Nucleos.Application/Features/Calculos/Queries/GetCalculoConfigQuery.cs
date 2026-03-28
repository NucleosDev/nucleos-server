using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calculos.DTOs;

namespace Nucleos.Application.Features.Calculos.Queries;

public class GetCalculoConfigQuery : IRequest<CalculoConfigDto?> { public Guid BlocoId { get; set; } }

public class GetCalculoConfigQueryHandler : IRequestHandler<GetCalculoConfigQuery, CalculoConfigDto?>
{
    private readonly IApplicationDbContext _context;
    public GetCalculoConfigQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<CalculoConfigDto?> Handle(GetCalculoConfigQuery request, CancellationToken ct)
    {
        var config = await _context.BlocoCalculos.FirstOrDefaultAsync(b => b.BlocoId == request.BlocoId, ct);
        if (config == null) return null;
        return new CalculoConfigDto { BlocoId = config.BlocoId, TipoOperacao = config.TipoOperacao, Campo = config.Campo, AgruparPor = config.AgruparPor };
    }
}
