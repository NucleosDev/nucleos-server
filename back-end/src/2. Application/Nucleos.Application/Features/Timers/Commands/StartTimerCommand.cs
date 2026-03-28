using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using System;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using System.Threading;
using Nucleos.Domain.Entities;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;
using AutoMapper;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Timers.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Timers.Commands;

public class StartTimerCommand : IRequest<TimerDto>
{
    public Guid NucleoId { get; set; }
    public string Titulo { get; set; }
}

public class StartTimerCommandHandler : IRequestHandler<StartTimerCommand, TimerDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public StartTimerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<TimerDto> Handle(StartTimerCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos.FindAsync(request.NucleoId);
        if (nucleo == null || nucleo.UserId != _currentUser.UserId)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var timer = new NucleoTimer
        {
            Id = Guid.NewGuid(),
            NucleoId = request.NucleoId,
            Titulo = request.Titulo,
            Inicio = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.NucleoTimers.AddAsync(timer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TimerDto>(timer);
    }
}