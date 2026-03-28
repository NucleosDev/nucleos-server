using MediatR;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.IA.DTOs;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Features.IA.Commands;

public class GerarInsightCommand : IRequest<InsightDto>
{
    public Guid? NucleoId { get; set; }
    public Guid UserId { get; set; }   // será usado para buscar contexto
}

public class GerarInsightCommandHandler : IRequestHandler<GerarInsightCommand, InsightDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IAIService _ai;
    private readonly ICurrentUserService _currentUser;

    public GerarInsightCommandHandler(IApplicationDbContext context, IAIService ai, ICurrentUserService currentUser)
    {
        _context = context;
        _ai = ai;
        _currentUser = currentUser;
    }

    public async Task<InsightDto> Handle(GerarInsightCommand request, CancellationToken cancellationToken)
    {
        var userId = request.UserId; // ou usar _currentUser.UserId, se for o mesmo

        // Buscar dados relevantes para o contexto (ex: tarefas recentes, hábitos, etc.)
        // Para simplificar, vamos criar um contexto simples
        var ultimasTarefas = await _context.Tarefas
            .Where(t => t.Bloco.Nucleo.UserId == userId && t.CreatedAt > DateTime.UtcNow.AddDays(-7))
            .ToListAsync(cancellationToken);

        var concluidas = ultimasTarefas.Count(t => t.Status == "concluida");
        var contextText = $"O usuário concluiu {concluidas} tarefas nos últimos 7 dias.";

        var insightText = await _ai.GerarInsightAsync(contextText, cancellationToken);

        var insight = new AIInsight
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            NucleoId = request.NucleoId,
            InsightType = "sugestao",
            Mensagem = insightText,
            Priority = 3,
            Applied = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.AiInsights.AddAsync(insight, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new InsightDto
        {
            Id = insight.Id,
            Mensagem = insight.Mensagem,
            InsightType = insight.InsightType,
            Priority = insight.Priority,
            CreatedAt = insight.CreatedAt
        };
    }
}