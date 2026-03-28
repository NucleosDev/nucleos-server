using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calendario.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calendario.Queries;

public class GetEventoByIdQuery : IRequest<CalendarioEventoDto>
{
    public Guid Id { get; set; }
}

public class GetEventoByIdQueryHandler : IRequestHandler<GetEventoByIdQuery, CalendarioEventoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetEventoByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CalendarioEventoDto> Handle(GetEventoByIdQuery request, CancellationToken ct)
    {
        var evento = await _context.CalendarioEventos
            .Include(e => e.Nucleo)
            .FirstOrDefaultAsync(e => e.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(CalendarioEvento), request.Id);

        if (evento.Nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        return _mapper.Map<CalendarioEventoDto>(evento);
    }
}
