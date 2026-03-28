using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Habitos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Queries;

public class GetHabitoByIdQuery : IRequest<HabitoDto>
{
    public Guid Id { get; set; }
}

public class GetHabitoByIdQueryHandler : IRequestHandler<GetHabitoByIdQuery, HabitoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetHabitoByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<HabitoDto> Handle(GetHabitoByIdQuery request, CancellationToken ct)
    {
        var habito = await _context.Habitos
            .Include(h => h.Bloco)
            .ThenInclude(b => b.Nucleo)
            .FirstOrDefaultAsync(h => h.Id == request.Id, ct)
            ?? throw new NotFoundException(nameof(Habito), request.Id);

        if (habito.Bloco.Nucleo.UserId != _currentUser.UserId)
            throw new ForbiddenException();

        return _mapper.Map<HabitoDto>(habito);
    }
}
