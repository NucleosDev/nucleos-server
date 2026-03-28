using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Gamificacao.DTOs;
using Nucleos.Application.Common.Exceptions;

namespace Nucleos.Application.Features.Gamificacao.Queries;

public class GetUserLevelQuery : IRequest<LevelDto> 
{ 
    public Guid UserId { get; set; } 
}

public class GetUserLevelQueryHandler : IRequestHandler<GetUserLevelQuery, LevelDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserLevelQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<LevelDto> Handle(GetUserLevelQuery request, CancellationToken ct)
    {
        // Verificar permissão: o usuário pode ver apenas seu próprio nível ou de outros?
        // Se for administrador, pode ver qualquer um; caso contrário, só o próprio.
        if (request.UserId != _currentUserService.UserId)
            throw new ForbiddenException();

        var level = await _context.UserLevels.FirstOrDefaultAsync(l => l.UserId == request.UserId, ct)
                    ?? throw new NotFoundException("UserLevel", request.UserId);

        return new LevelDto
        {
            UserId = level.UserId ?? request.UserId,
            Level = level.Level,
            CurrentXp = level.CurrentXp,
            NextLevelXp = level.NextLevelXp,
            TotalXpEarned = level.TotalXpEarned
        };
    }
}