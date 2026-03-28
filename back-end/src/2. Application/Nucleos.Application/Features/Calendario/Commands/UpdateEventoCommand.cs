using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calendario.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Calendario.Commands;

public class UpdateEventoCommand : IRequest<CalendarioEventoDto>
{
    public Guid Id { get; set; }
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
    public DateTime? DataEvento { get; set; }
    public int? DuracaoMinutos { get; set; }
}

public class UpdateEventoCommandHandler : IRequestHandler<UpdateEventoCommand, CalendarioEventoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UpdateEventoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CalendarioEventoDto> Handle(UpdateEventoCommand request, CancellationToken ct)
    {
        var evento = await _context.CalendarioEventos
            .Include(e => e.Nucleo)
            .FirstOrDefaultAsync(e => e.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(CalendarioEvento), request.Id);

        if (evento.Nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        if (request.Titulo != null) evento.Titulo = request.Titulo;
        if (request.Descricao != null) evento.Descricao = request.Descricao;
        if (request.DataEvento.HasValue) evento.DataEvento = request.DataEvento;
        if (request.DuracaoMinutos.HasValue) evento.DuracaoMinutos = request.DuracaoMinutos;
        evento.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return _mapper.Map<CalendarioEventoDto>(evento);
    }
}
