using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;

namespace Nucleos.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // ========== USUÁRIOS E AUTENTICAÇÃO ==========
    DbSet<User> Users { get; }
    DbSet<UserProfile> UserProfiles { get; }
    DbSet<UserSecurity> UserSecurity { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserPreference> UserPreferences { get; }
    DbSet<PasswordReset> PasswordResets { get; }

    // ========== NÚCLEOS ==========
    DbSet<Nucleo> Nucleos { get; }
    DbSet<NucleoIcon> NucleoIcons { get; }
    DbSet<NucleoCompartilhamento> NucleoCompartilhamentos { get; }
    DbSet<NucleoRelation> NucleoRelations { get; }
    DbSet<NucleoAchievement> NucleoAchievements { get; }

    // ========== BLOCOS ==========
    DbSet<Bloco> Blocos { get; }
    DbSet<BlocoCalculo> BlocoCalculos { get; }

    // ========== LISTAS E ITENS ==========
    DbSet<Lista> Listas { get; }
    DbSet<Categoria> Categorias { get; }
    DbSet<ItemLista> ItensLista { get; }

    // ========== COLEÇÕES DINÂMICAS ==========
    DbSet<Colecao> Colecoes { get; }
    DbSet<Campo> Campos { get; }
    DbSet<Item> Itens { get; }
    DbSet<ItemValor> ItemValores { get; }

    // ========== TAREFAS ==========
    DbSet<Tarefa> Tarefas { get; }

    // ========== HÁBITOS ==========
    DbSet<Habito> Habitos { get; }
    DbSet<HabitoRegistro> HabitosRegistros { get; }

    // ========== CALENDÁRIO ==========
    DbSet<CalendarioEvento> CalendarioEventos { get; }

    // ========== TIMERS ==========
    DbSet<NucleoTimer> NucleoTimers { get; }

    // ========== METAS ==========
    DbSet<Meta> Metas { get; }

    // ========== GAMIFICAÇÃO ==========
    DbSet<Conquista> Conquistas { get; }
    DbSet<UserConquista> UserConquistas { get; }
    DbSet<UserLevel> UserLevels { get; }
    DbSet<Streak> Streaks { get; }
    DbSet<XP_Log> XpLogs { get; }
    DbSet<EnergyLog> EnergyLogs { get; }

    // ========== NOTIFICAÇÕES ==========
    DbSet<Notification> Notifications { get; }
    DbSet<ActivityLog> ActivityLogs { get; }

    // ========== ASSINATURAS ==========
    DbSet<Plan> Plans { get; }
    DbSet<Subscription> Subscriptions { get; }

    // ========== IA ==========
    DbSet<AIInteraction> AiInteractions { get; }
    DbSet<AIContext> AiContext { get; }
    DbSet<AIInsight> AiInsights { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}