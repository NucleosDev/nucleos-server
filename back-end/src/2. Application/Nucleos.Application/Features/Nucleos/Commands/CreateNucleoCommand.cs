using MediatR;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nucleos.Application.Features.Nucleos.DTOs;
using Nucleos.Application.Common.Exceptions;

namespace Nucleos.Application.Features.Nucleos.Commands;

public class CreateNucleoCommand : IRequest<NucleoDto>
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; } = "pessoal";
    public string CorDestaque { get; set; }
    public string ImagemCapa { get; set; }
    public Guid? IconId { get; set; }
}

public class CreateNucleoCommandHandler : IRequestHandler<CreateNucleoCommand, NucleoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateNucleoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<NucleoDto> Handle(CreateNucleoCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId ?? throw new UnauthorizedException();

        var nucleo = new Nucleo
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Tipo = request.Tipo,
            CorDestaque = request.CorDestaque,
            ImagemCapa = request.ImagemCapa,
            IconId = request.IconId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Nucleos.AddAsync(nucleo, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NucleoDto>(nucleo);
    }
}