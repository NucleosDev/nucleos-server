using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Timers.DTOs;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Application.Common.Exceptions;
using AutoMapper;

namespace Nucleos.Application.Features.Timers.Commands;

public class StopTimerCommand : IRequest<TimerDto>
{
    public Guid Id { get; set; }
}

public class StopTimerCommandHandler : IRequestHandler<StopTimerCommand, TimerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IGamificationEngine _gamification;
    private readonly IMapper _mapper;

    public StopTimerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IGamificationEngine gamification, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _gamification = gamification;
        _mapper = mapper;
    }

    public async Task<TimerDto> Handle(StopTimerCommand request, CancellationToken cancellationToken)
    {
        var timer = await _context.NucleoTimers
            .Include(t => t.Nucleo)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (timer == null)
            throw new NotFoundException(nameof(NucleoTimer), request.Id);

        if (timer.Nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        if (!timer.Inicio.HasValue)
            throw new InvalidOperationException("Timer was never started.");

        timer.Fim = DateTime.UtcNow;
        var duracao = timer.Fim.Value - timer.Inicio.Value;
        timer.DuracaoSegundos = (int)duracao.TotalSeconds;
        timer.UpdatedAt = DateTime.UtcNow;

        // Adiciona XP baseado no tempo focado (1 XP por minuto)
        int seconds = timer.DuracaoSegundos ?? 0;
        int xp = seconds / 60;
        if (xp > 0)
            await _gamification.AddXp(timer.Nucleo.UserId, xp, cancellationToken);  // supondo 3 parâmetros

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TimerDto>(timer);
    }
}