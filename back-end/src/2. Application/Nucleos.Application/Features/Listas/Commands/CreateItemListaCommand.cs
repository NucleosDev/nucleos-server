using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.ItensLista.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Listas.Commands;

public class CreateItemListaCommand : IRequest<ItemListaDto>
{
    public Guid ListaId { get; set; }
    public string Nome { get; set; }
    public decimal Quantidade { get; set; } = 1;
    public decimal? ValorUnitario { get; set; }
    public bool Checked { get; set; } = false;
}

public class CreateItemListaCommandHandler : IRequestHandler<CreateItemListaCommand, ItemListaDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateItemListaCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<ItemListaDto> Handle(CreateItemListaCommand request, CancellationToken cancellationToken)
    {
        var lista = await _context.Listas.FindAsync(request.ListaId);
        if (lista == null)
            throw new NotFoundException(nameof(Lista), request.ListaId);

        var bloco = await _context.Blocos.FindAsync(lista.BlocoId);
        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), lista.BlocoId);

        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId && !await IsSharedWithWrite(nucleo.Id))
            throw new ForbiddenException();

        var item = new ItemLista
        {
            Id = Guid.NewGuid(),
            ListaId = request.ListaId,
            Nome = request.Nome,
            Quantidade = request.Quantidade,
            ValorUnitario = request.ValorUnitario,
            Checked = request.Checked,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ValorTotal = request.Quantidade * (request.ValorUnitario ?? 0)
        };

        await _context.ItensLista.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ItemListaDto>(item);
    }

    private async Task<bool> IsSharedWithWrite(Guid nucleoId)
    {
        var userId = _currentUser.UserId ?? throw new ForbiddenException();
        return await _context.NucleoCompartilhamentos
            .AnyAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == userId && c.PermissionLevel == "edit");
    }
}