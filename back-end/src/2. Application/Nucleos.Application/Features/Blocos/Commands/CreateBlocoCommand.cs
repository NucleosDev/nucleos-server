using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Blocos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Blocos.Commands;

public class CreateBlocoCommand : IRequest<BlocoDto>
{
    public Guid NucleoId { get; set; }
    public string Tipo { get; set; }
    public string Titulo { get; set; }
    public int Posicao { get; set; }
}

public class CreateBlocoCommandHandler : IRequestHandler<CreateBlocoCommand, BlocoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateBlocoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<BlocoDto> Handle(CreateBlocoCommand request, CancellationToken cancellationToken)
    {
        var nucleo = await _context.Nucleos.FindAsync(request.NucleoId);
        if (nucleo == null || nucleo.UserId != _currentUser.UserId)
            throw new NotFoundException(nameof(Nucleo), request.NucleoId);

        var bloco = new Bloco
        {
            Id = Guid.NewGuid(),
            NucleoId = request.NucleoId,
            Tipo = request.Tipo,
            Titulo = request.Titulo,
            Posicao = request.Posicao,
            Configuracoes = "{}",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Blocos.AddAsync(bloco, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<BlocoDto>(bloco);
    }
}