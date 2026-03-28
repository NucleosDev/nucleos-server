using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Habitos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Commands;

public class CreateHabitoCommand : IRequest<HabitoDto>
{
    public Guid BlocoId { get; set; }
    public string Nome { get; set; }
    public string Frequencia { get; set; }
    public int? MetaVezes { get; set; }
    public int[] DiasSemana { get; set; } = Array.Empty<int>();
}

public class CreateHabitoCommandHandler : IRequestHandler<CreateHabitoCommand, HabitoDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public CreateHabitoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    // Verifica se o usuário tem permissão de escrita no núcleo
    private async Task<bool> HasWriteAccess(Guid nucleoId)
    {
        var compartilhamento = await _context.NucleoCompartilhamentos
            .FirstOrDefaultAsync(c => c.NucleoId == nucleoId && c.SharedWithUserId == _currentUser.UserId);
        return compartilhamento != null && compartilhamento.PermissionLevel == "edit";
    }

    public async Task<HabitoDto> Handle(CreateHabitoCommand request, CancellationToken cancellationToken)
    {
        var bloco = await _context.Blocos.FindAsync(request.BlocoId);
        if (bloco == null)
            throw new NotFoundException(nameof(Bloco), request.BlocoId);

        var nucleo = await _context.Nucleos.FindAsync(bloco.NucleoId);
        if (nucleo.UserId != _currentUser.UserId && !await HasWriteAccess(nucleo.Id))
            throw new ForbiddenException();

        var habito = new Habito
        {
            Id = Guid.NewGuid(),
            BlocoId = request.BlocoId,
            Nome = request.Nome,
            Frequencia = request.Frequencia,
            MetaVezes = request.MetaVezes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.Habitos.AddAsync(habito, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<HabitoDto>(habito);
        dto.DiasSemana = request.DiasSemana is { Length: > 0 } ? request.DiasSemana.ToList() : null;
        return dto;
    }
}