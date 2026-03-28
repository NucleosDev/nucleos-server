using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Admin.DTOs;

namespace Nucleos.Application.Features.Admin.Queries;

public class GetUsersQuery : IRequest<List<AdminUserDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<AdminUserDto>>
{
    private readonly IApplicationDbContext _context;
    public GetUsersQueryHandler(IApplicationDbContext context) => _context = context;

    public async Task<List<AdminUserDto>> Handle(GetUsersQuery request, CancellationToken ct)
        => await _context.Users
            .Include(u => u.Profile)
            .Include(u => u.Roles)
            .Where(u => u.DeletedAt == null)
            .OrderByDescending(u => u.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new AdminUserDto { Id = u.Id, Email = u.Email, FullName = u.Profile != null ? u.Profile.FullName : "", Active = u.Active, CreatedAt = u.CreatedAt, Role = u.Roles.FirstOrDefault() != null ? u.Roles.First().Role : "user" })
            .ToListAsync(ct);
}
