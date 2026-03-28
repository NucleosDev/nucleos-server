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

namespace Nucleos.Application.Features.Nucleos.Commands;

public class UpdateNucleoCommand : IRequest<NucleoDto>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; }
    public string CorDestaque { get; set; }
    public string ImagemCapa { get; set; }
    public Guid? IconId { get; set; }
}

public class UpdateNucleoCommandHandler : IRequestHandler<UpdateNucleoCommand, NucleoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UpdateNucleoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<NucleoDto> Handle(UpdateNucleoCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos
            .FirstOrDefaultAsync(n => n.Id == request.Id && n.UserId == _currentUser.UserId, cancellationToken);

        if (nucleo == null)
            throw new NotFoundException(nameof(Nucleo), request.Id);

        nucleo.Nome = request.Nome;
        nucleo.Descricao = request.Descricao;
        nucleo.Tipo = request.Tipo;
        nucleo.CorDestaque = request.CorDestaque;
        nucleo.ImagemCapa = request.ImagemCapa;
        nucleo.IconId = request.IconId;
        nucleo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NucleoDto>(nucleo);
    }
}