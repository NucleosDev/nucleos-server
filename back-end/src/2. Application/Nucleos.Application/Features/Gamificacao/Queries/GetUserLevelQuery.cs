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
        if (!_currentUserService.HasPermission("ViewUserLevel") && 
            request.UserId != _currentUserService.UserId)
        {
            throw new ForbiddenException();
        }
        
        // Buscar nível do usuário
        var level = await _context.UserLevels.FirstOrDefaultAsync(l => l.UserId == request.UserId, ct);
        
        // Se não existir nível para o usuário, retorna um padrão (não lança exceção)
        if (level == null)
        {
            return new LevelDto
            {
                UserId = request.UserId,
                Level = 1,
                CurrentXp = 0,
                NextLevelXp = 100,
                TotalXpEarned = 0
            };
        }

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