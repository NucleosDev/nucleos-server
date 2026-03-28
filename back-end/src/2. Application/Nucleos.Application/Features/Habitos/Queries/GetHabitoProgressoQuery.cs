using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Habitos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Queries;

public class GetHabitoProgressoQuery : IRequest<HabitoProgressoDto>
{
    public Guid Id { get; set; }
}

public class GetHabitoProgressoQueryHandler : IRequestHandler<GetHabitoProgressoQuery, HabitoProgressoDto>
{
    private readonly IApplicationDbContext _context;
    public GetHabitoProgressoQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<HabitoProgressoDto> Handle(GetHabitoProgressoQuery request, CancellationToken ct)
    {
        var habito = await _context.Habitos
                         .FirstOrDefaultAsync(h => h.Id == request.Id, ct)
                     ?? throw new NotFoundException(nameof(Habito), request.Id);

        // Contar registros dos últimos 30 dias, por exemplo
        var registros = await _context.HabitosRegistros
            .Where(r => r.HabitoId == request.Id && r.Data >= DateTime.UtcNow.AddDays(-30))
            .ToListAsync(ct);

        var totalCompletados = registros.Sum(r => r.VezesCompletadas);
        var mediaDiaria = totalCompletados / 30.0;

        return new HabitoProgressoDto
        {
            HabitoId = habito.Id,
            Nome = habito.Nome,
            TotalCompletados = totalCompletados,
            MediaDiaria = mediaDiaria,
            Registros = registros.Select(r => new RegistroDto { Data = r.Data, Vezes = r.VezesCompletadas }).ToList()
        };
    }
}