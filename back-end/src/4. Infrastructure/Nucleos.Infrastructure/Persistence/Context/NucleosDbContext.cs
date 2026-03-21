using Microsoft.EntityFrameworkCore;

namespace Nucleos.Infrastructure.Persistence.Context;

public class NucleosDbContext : DbContext
{
    public NucleosDbContext(DbContextOptions<NucleosDbContext> options) : base(options)
    {
    }

    // Todas as tabelas do Supabase
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSecurity> UserSecurity { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<PasswordReset> PasswordResets { get; set; }
    
    public DbSet<Nucleo> Nucleos { get; set; }
    public DbSet<NucleoIcon> NucleoIcons { get; set; }
    public DbSet<NucleoCompartilhamento> NucleoCompartilhamentos { get; set; }
    public DbSet<NucleoRelation> NucleoRelations { get; set; }
    public DbSet<NucleoAchievement> NucleoAchievements { get; set; }
    
    public DbSet<Bloco> Blocos { get; set; }
    public DbSet<BlocoCalculo> BlocoCalculos { get; set; }
    
    public DbSet<Lista> Listas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<ItemLista> ItensLista { get; set; }
    
    public DbSet<Tarefa> Tarefas { get; set; }
    
    public DbSet<Habito> Habitos { get; set; }
    public DbSet<HabitoRegistro> HabitosRegistros { get; set; }
    
    public DbSet<Timer> Timers { get; set; }
    public DbSet<CalendarioEvento> CalendarioEventos { get; set; }
    public DbSet<Meta> Metas { get; set; }
    
    public DbSet<Conquista> Conquistas { get; set; }
    public DbSet<UserConquista> UserConquistas { get; set; }
    public DbSet<UserLevel> UserLevels { get; set; }
    public DbSet<Streak> Streaks { get; set; }
    public DbSet<XP_Log> XpLogs { get; set; }
    public DbSet<EnergyLog> EnergyLogs { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    
    public DbSet<AIInteraction> AiInteractions { get; set; }
    public DbSet<AIContext> AiContext { get; set; }
    public DbSet<AIInsight> AiInsights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Cpf).HasMaxLength(14);
            entity.HasIndex(e => e.Cpf).IsUnique();
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
        });

        modelBuilder.Entity<UserSecurity>(entity =>
        {
            entity.ToTable("user_security");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.ToTable("user_preferences");
            entity.HasKey(e => e.UserId);
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.ToTable("password_resets");
            entity.HasKey(e => e.Id);
        });

        // Nucleos
        modelBuilder.Entity<Nucleo>(entity =>
        {
            entity.ToTable("nucleos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.User).WithMany(u => u.Nucleos).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Icon).WithMany(i => i.Nucleos).HasForeignKey(e => e.IconId);
        });

        modelBuilder.Entity<NucleoIcon>(entity =>
        {
            entity.ToTable("nucleo_icons");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<NucleoCompartilhamento>(entity =>
        {
            entity.ToTable("nucleo_compartilhamentos");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<NucleoRelation>(entity =>
        {
            entity.ToTable("nucleo_relations");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<NucleoAchievement>(entity =>
        {
            entity.ToTable("nucleo_achievements");
            entity.HasKey(e => e.Id);
        });

        // Blocos
        modelBuilder.Entity<Bloco>(entity =>
        {
            entity.ToTable("blocos");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Nucleo).WithMany(n => n.Blocos).HasForeignKey(e => e.NucleoId);
        });

        modelBuilder.Entity<BlocoCalculo>(entity =>
        {
            entity.ToTable("bloco_calculos");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.BlocoId).IsUnique();
        });

        // Listas
        modelBuilder.Entity<Lista>(entity =>
        {
            entity.ToTable("listas");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Bloco).WithMany(b => b.Listas).HasForeignKey(e => e.BlocoId);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ItemLista>(entity =>
        {
            entity.ToTable("itens_lista");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Lista).WithMany(l => l.Itens).HasForeignKey(e => e.ListaId);
            entity.HasOne(e => e.Categoria).WithMany(c => c.Itens).HasForeignKey(e => e.CategoriaId);
        });

        // Tarefas
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("tarefas");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Bloco).WithMany(b => b.Tarefas).HasForeignKey(e => e.BlocoId);
        });

        // Habitos
        modelBuilder.Entity<Habito>(entity =>
        {
            entity.ToTable("habitos");
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Bloco).WithMany(b => b.Habitos).HasForeignKey(e => e.BlocoId);
        });

        modelBuilder.Entity<HabitoRegistro>(entity =>
        {
            entity.ToTable("habitos_registros");
            entity.HasKey(e => e.Id);
        });

        // Timers
        modelBuilder.Entity<Timer>(entity =>
        {
            entity.ToTable("timers");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<CalendarioEvento>(entity =>
        {
            entity.ToTable("calendario_eventos");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Meta>(entity =>
        {
            entity.ToTable("metas");
            entity.HasKey(e => e.Id);
        });

        // Gamificação
        modelBuilder.Entity<Conquista>(entity =>
        {
            entity.ToTable("conquistas");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<UserConquista>(entity =>
        {
            entity.ToTable("user_conquistas");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<UserLevel>(entity =>
        {
            entity.ToTable("user_levels");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Streak>(entity =>
        {
            entity.ToTable("streaks");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<XP_Log>(entity =>
        {
            entity.ToTable("xp_logs");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<EnergyLog>(entity =>
        {
            entity.ToTable("energy_logs");
            entity.HasKey(e => e.Id);
        });

        // Notificações
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.ToTable("activity_logs");
            entity.HasKey(e => e.Id);
        });

        // Planos
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("plans");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");
            entity.HasKey(e => e.Id);
        });

        // AI
        modelBuilder.Entity<AIInteraction>(entity =>
        {
            entity.ToTable("ai_interactions");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<AIContext>(entity =>
        {
            entity.ToTable("ai_context");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId).IsUnique();
        });

        modelBuilder.Entity<AIInsight>(entity =>
        {
            entity.ToTable("ai_insights");
            entity.HasKey(e => e.Id);
        });
    }
}
