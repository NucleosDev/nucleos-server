using MediatR;
using Nucleos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Exceptions;
using Nucleos.Domain.Entities;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;
using Nucleos.Application.Features.Habitos.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.Habitos.Commands;

public class UpdateHabitoCommand : IRequest<HabitoDto>
{
    public Guid Id { get; set; }
    public string? Nome { get; set; }
    public string? Frequencia { get; set; }
    public int? MetaVezes { get; set; }
    public List<int>? DiasSemana { get; set; }  // array de inteiros (0-6)
}

public class UpdateHabitoCommandHandler : IRequestHandler<UpdateHabitoCommand, HabitoDto>
{
    private readonly IApplicationDbContext _context;
    public UpdateHabitoCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<HabitoDto> Handle(UpdateHabitoCommand request, CancellationToken ct)
    {
        var habito = await _context.Habitos
                         .FirstOrDefaultAsync(h => h.Id == request.Id, ct)  // habitos não tem soft delete
                     ?? throw new NotFoundException(nameof(Habito), request.Id);

        if (request.Nome != null) habito.Nome = request.Nome;
        if (request.Frequencia != null) habito.Frequencia = request.Frequencia;
        if (request.MetaVezes.HasValue) habito.MetaVezes = request.MetaVezes.Value;
        habito.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return new HabitoDto
        {
            Id = habito.Id,
            BlocoId = habito.BlocoId,
            Nome = habito.Nome,
            Frequencia = habito.Frequencia,
            DiasSemana = request.DiasSemana,
            MetaVezes = habito.MetaVezes,
            CreatedAt = habito.CreatedAt
        };
    }
}