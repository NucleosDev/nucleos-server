using MediatR;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calculos.DTOs;
using Nucleos.Application.Features.Calculos.Services;
using Microsoft.EntityFrameworkCore;

namespace Nucleos.Application.Features.Calculos.Queries;

public class GetCalculoResultadoQuery : IRequest<CalculoResultadoDto> { public Guid BlocoId { get; set; } }

public class GetCalculoResultadoQueryHandler : IRequestHandler<GetCalculoResultadoQuery, CalculoResultadoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICalculoEngine _engine;
    public GetCalculoResultadoQueryHandler(IApplicationDbContext context, ICalculoEngine engine) { _context = context; _engine = engine; }

    public async Task<CalculoResultadoDto> Handle(GetCalculoResultadoQuery request, CancellationToken ct)
    {
        var config = await _context.BlocoCalculos.FirstOrDefaultAsync(b => b.BlocoId == request.BlocoId, ct)
            ?? throw new Common.Exceptions.NotFoundException("BlocoCalculo", request.BlocoId);
        var resultado = await _engine.ExecutarAsync(request.BlocoId, config.TipoOperacao, config.Campo, config.AgruparPor, ct);
        return new CalculoResultadoDto
        {
            Valor = (decimal)resultado,
            Detalhes = $"Operação {config.TipoOperacao} no bloco {request.BlocoId} em {DateTime.UtcNow:O}"
        };
    }
}
