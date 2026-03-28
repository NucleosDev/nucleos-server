using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Listas.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Listas.Commands;

public class CreateListaCommand : IRequest<ListaDto>
{
    public Guid BlocoId { get; set; }
    public string Nome { get; set; }
    public string TipoLista { get; set; } = "generica";
}

public class CreateListaCommandHandler : IRequestHandler<CreateListaCommand, ListaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateListaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<ListaDto> Handle(CreateListaCommand request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos.FindAsync(request.BlocoId);
        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.BlocoId);

        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId && !await IsSharedWithWrite(nucleo.Id))
            throw new ForbiddenException();

        var lista = new Lista
        {
            Id = Guid.NewGuid(),
            BlocoId = request.BlocoId,
            Nome = request.Nome,
            TipoLista = request.TipoLista,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Listas.AddAsync(lista, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ListaDto>(lista);
    }

    private async Task<bool> IsSharedWithWrite(Guid nucleoId)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenException();
        return await _context.NucleoCompartilhamentos
            .AnyAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == userId && c.PermissionLevel == "edit");
    }
}