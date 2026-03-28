using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Timers.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Timers.Queries;

public class GetTimersByNucleoQuery : IRequest<List<TimerDto>>
{
    public Guid NucleoId { get; set; }
}

public class GetTimersByNucleoQueryHandler : IRequestHandler<GetTimersByNucleoQuery, List<TimerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetTimersByNucleoQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<TimerDto>> Handle(GetTimersByNucleoQuery request, CancellationToken ct)
    {
        var nucleo = await _context.Nucleos.FindAsync(new object[] { request.NucleoId }, ct);
        if (nucleo == null || nucleo.UserId != _currentUser.UserId)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var timers = await _context.NucleoTimers.Where(t => t.NucleoId == request.NucleoId).ToListAsync(ct);
        return _mapper.Map<List<TimerDto>>(timers);
    }
}
