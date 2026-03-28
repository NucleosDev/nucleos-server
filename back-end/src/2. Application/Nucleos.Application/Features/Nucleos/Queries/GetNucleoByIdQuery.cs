using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Entities;
using System.Threading;
using Nucleos.Domain.Entities;
using System.Threading.Tasks;
using Nucleos.Domain.Entities;
using AutoMapper;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Nucleos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Nucleos.Queries;

public class GetNucleoByIdQuery : IRequest<NucleoDetailDto>
{
    public Guid Id { get; set; }
}

public class GetNucleoByIdQueryHandler : IRequestHandler<GetNucleoByIdQuery, NucleoDetailDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetNucleoByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<NucleoDetailDto> Handle(GetNucleoByIdQuery request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .Include(n => n.Blocos.Where(b => b.DeletedAt == null))
                .ThenInclude(b => b.Listas.Where(l => l.DeletedAt == null))
                .ThenInclude(l => l.Itens.Where(i => i.DeletedAt == null))
            .Include(n => n.Blocos.Where(b => b.DeletedAt == null))
                .ThenInclude(b => b.Tarefas.Where(t => t.DeletedAt == null))
            .Include(n => n.Blocos.Where(b => b.DeletedAt == null))
                .ThenInclude(b => b.Habitos)
            .Include(n => n.Blocos.Where(b => b.DeletedAt == null))
                .ThenInclude(b => b.Calculo)
            .Include(n => n.Blocos.Where(b => b.DeletedAt == null))
                .ThenInclude(b => b.Colecoes)
                .ThenInclude(c => c.Campos)
            .FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.Id);

        return _mapper.Map<NucleoDetailDto>(nucleo);
    }
}