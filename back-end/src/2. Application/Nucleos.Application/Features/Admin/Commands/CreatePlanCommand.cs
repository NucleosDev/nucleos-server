using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Admin.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Admin.Commands;

public class CreatePlanCommand : IRequest<PlanDto>
{
    public string Name { get; set; }
    public int MaxNucleos { get; set; }
    public decimal Price { get; set; }
}

public class CreatePlanCommandHandler : IRequestHandler<CreatePlanCommand, PlanDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreatePlanCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<PlanDto> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.UserId == userId && r.Role == "Admin", cancellationToken);
        if (userRole == null)
            throw new ForbiddenException();

        var plan = new Plan
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            MaxNucleos = request.MaxNucleos,
            Price = request.Price,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Plans.AddAsync(plan, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PlanDto>(plan);
    }
}