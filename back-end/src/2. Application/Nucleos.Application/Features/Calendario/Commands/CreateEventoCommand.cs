using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calendario.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calendario.Commands;

public class CreateEventoCommand : IRequest<CalendarioEventoDto>
{
    public Guid NucleoId { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public DateTime DataEvento { get; set; }
    public int DuracaoMinutos { get; set; } = 60;
}

public class CreateEventoCommandHandler : IRequestHandler<CreateEventoCommand, CalendarioEventoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateEventoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CalendarioEventoDto> Handle(CreateEventoCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos.FindAsync(request.NucleoId);
        if (nucleo == null || nucleo.UserId != _currentUser.UserId)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var evento = new CalendarioEvento
        {
            Id = Guid.NewGuid(),
            NucleoId = request.NucleoId,
            Titulo = request.Titulo,
            Descricao = request.Descricao,
            DataEvento = request.DataEvento,
            DuracaoMinutos = request.DuracaoMinutos,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.CalendarioEventos.AddAsync(evento, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CalendarioEventoDto>(evento);
    }
}