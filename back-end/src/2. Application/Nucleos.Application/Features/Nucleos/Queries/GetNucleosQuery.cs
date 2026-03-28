using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nucleos.Application.Features.Nucleos.DTOs;

namespace Nucleos.Application.Features.Nucleos.Queries;

public class GetNucleosQuery : IRequest<List<NucleoDto>>
{
}

public class GetNucleosQueryHandler : IRequestHandler<GetNucleosQuery, List<NucleoDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetNucleosQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<List<NucleoDto>> Handle(GetNucleosQuery request, CancellationToken cancellationToken)
    {
        var nucleos = await _context.Nucleos
            .Where(n => n.UserId == _currentUser.UserId && n.DeletedAt == null)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<NucleoDto>>(nucleos);
    }
}