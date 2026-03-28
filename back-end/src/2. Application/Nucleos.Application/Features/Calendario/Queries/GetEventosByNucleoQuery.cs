using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calendario.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calendario.Queries;

public class GetEventosByNucleoQuery : IRequest<List<CalendarioEventoDto>>
{
    public Guid NucleoId { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}

public class GetEventosByNucleoQueryHandler : IRequestHandler<GetEventosByNucleoQuery, List<CalendarioEventoDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetEventosByNucleoQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<CalendarioEventoDto>> Handle(GetEventosByNucleoQuery request, CancellationToken ct)
    {
        var nucleo = await _context.Nucleos.FindAsync(new object[] { request.NucleoId }, ct);
        if (nucleo == null || nucleo.UserId != _currentUser.UserId)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var q = _context.CalendarioEventos.Where(e => e.NucleoId == request.NucleoId);
        if (request.Start.HasValue)
            q = q.Where(e => e.DataEvento >= request.Start);
        if (request.End.HasValue)
            q = q.Where(e => e.DataEvento <= request.End);

        var list = await q.OrderBy(e => e.DataEvento).ToListAsync(ct);
        return _mapper.Map<List<CalendarioEventoDto>>(list);
    }
}
