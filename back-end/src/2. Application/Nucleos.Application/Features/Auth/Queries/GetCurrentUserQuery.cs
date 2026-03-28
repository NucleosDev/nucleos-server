using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Auth.DTOs;

namespace Nucleos.Application.Features.Auth.Queries;

public class GetCurrentUserQuery : IRequest<UserDto> { public Guid UserId { get; set; } }

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    public GetCurrentUserQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var user = await _context.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == request.UserId, ct)
            ?? throw new NotFoundException("User", request.UserId);
        return new UserDto { Id = user.Id, Email = user.Email, FullName = user.Profile?.FullName ?? "", Nickname = user.Profile?.Nickname, AvatarUrl = user.Profile?.AvatarUrl };
    }
}
