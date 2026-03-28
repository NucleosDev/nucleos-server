using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Timers.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Timers.Commands;

public class ResumeTimerCommand : IRequest<TimerDto>
{
    public Guid Id { get; set; }
}

public class ResumeTimerCommandHandler : IRequestHandler<ResumeTimerCommand, TimerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public ResumeTimerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<TimerDto> Handle(ResumeTimerCommand request, CancellationToken ct)
    {
        var timer = await _context.NucleoTimers
            .Include(t => t.Nucleo)
            .FirstOrDefaultAsync(t => t.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(NucleoTimer), request.Id);

        if (timer.Nucleo!.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        if (!timer.Inicio.HasValue && !timer.Fim.HasValue)
        {
            timer.Inicio = DateTime.UtcNow;
            timer.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
        }

        return _mapper.Map<TimerDto>(timer);
    }
}
