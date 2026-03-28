using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Gamification;

public class ConquistaChecker : IConquistaChecker
{
    private readonly IApplicationDbContext _context;

    public ConquistaChecker(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CheckAndUnlock(Guid userId, string eventType, object data)
    {
        var conquistas = await _context.Conquistas
            .Where(c => c.Tipo == eventType)
            .ToListAsync();

        foreach (var conquista in conquistas)
        {
            bool unlocked = false;
            // Exemplo de condição simples: { "minTarefas": 10 }
            // Você pode implementar a avaliação de condições com System.Text.Json
            // Por enquanto, só um placeholder
            // ...

            if (unlocked)
            {
                var userConquista = new UserConquista
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ConquistaId = conquista.Id,
                    DesbloqueadoEm = DateTime.UtcNow
                };
                await _context.UserConquistas.AddAsync(userConquista);
            }
        }
        await _context.SaveChangesAsync();
    }
}