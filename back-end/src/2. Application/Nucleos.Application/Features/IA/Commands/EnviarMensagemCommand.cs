using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.IA.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.IA.Commands;

public class EnviarMensagemCommand : IRequest<MensagemDto>
{
    public Guid UserId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}

public class EnviarMensagemCommandHandler : IRequestHandler<EnviarMensagemCommand, MensagemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _aiService;
    public EnviarMensagemCommandHandler(IApplicationDbContext context, IAIService aiService)
    { _context = context; _aiService = aiService; }

    public async Task<MensagemDto> Handle(EnviarMensagemCommand request, CancellationToken ct)
    {
        var resposta = await _aiService.EnviarMensagemAsync(request.Mensagem, null, ct);
        var interaction = new AIInteraction
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Mensagem = request.Mensagem,
            Resposta = resposta,
            CreatedAt = DateTime.UtcNow
        };
        await _context.AiInteractions.AddAsync(interaction, ct);
        await _context.SaveChangesAsync(ct);
        return new MensagemDto { Role = "assistant", Conteudo = resposta, Timestamp = DateTime.UtcNow };
    }
}
