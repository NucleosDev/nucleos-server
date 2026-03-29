using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Domain.Entities;

namespace Nucleos.Infrastructure.Persistence.Context;

public class NucleosDbContext : DbContext, IApplicationDbContext
{
    public NucleosDbContext(DbContextOptions<NucleosDbContext> options) : base(options)
    {
    }

    // ========== USUÁRIOS E AUTENTICAÇÃO ==========
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSecurity> UserSecurity { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserPreference> UserPreferences { get; set; }
    public DbSet<PasswordReset> PasswordResets { get; set; }
    
    // ========== NÚCLEOS ==========
    public DbSet<Nucleo> Nucleos { get; set; }
    public DbSet<NucleoIcon> NucleoIcons { get; set; }
    public DbSet<NucleoCompartilhamento> NucleoCompartilhamentos { get; set; }
    public DbSet<NucleoRelation> NucleoRelations { get; set; }
    public DbSet<NucleoAchievement> NucleoAchievements { get; set; }
    
    // ========== BLOCOS ==========
    public DbSet<Bloco> Blocos { get; set; }
    public DbSet<BlocoCalculo> BlocoCalculos { get; set; }
    
    // ========== LISTAS E ITENS ==========
    public DbSet<Lista> Listas { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<ItemLista> ItensLista { get; set; }
    
    // ========== COLEÇÕES DINÂMICAS ==========
    public DbSet<Colecao> Colecoes { get; set; }
    public DbSet<Campo> Campos { get; set; }
    public DbSet<Item> Itens { get; set; }
    public DbSet<ItemValor> ItemValores { get; set; }
    
    // ========== TAREFAS ==========
    public DbSet<Tarefa> Tarefas { get; set; }
    
    // ========== HÁBITOS ==========
    public DbSet<Habito> Habitos { get; set; }
    public DbSet<HabitoRegistro> HabitosRegistros { get; set; }
    
    // ========== CALENDÁRIO ==========
    public DbSet<CalendarioEvento> CalendarioEventos { get; set; }
    
    // ========== TIMERS ==========
    public DbSet<NucleoTimer> NucleoTimers { get; set; }
    
    // ========== METAS ==========
    public DbSet<Meta> Metas { get; set; }
    
    // ========== GAMIFICAÇÃO ==========
    public DbSet<Conquista> Conquistas { get; set; }
    public DbSet<UserConquista> UserConquistas { get; set; }
    public DbSet<UserLevel> UserLevels { get; set; }
    public DbSet<Streak> Streaks { get; set; }
    public DbSet<XP_Log> XpLogs { get; set; }
    public DbSet<EnergyLog> EnergyLogs { get; set; }
    
    // ========== NOTIFICAÇÕES ==========
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    
    // ========== ASSINATURAS ==========
    public DbSet<Plan> Plans { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    
    // ========== IA ==========
    public DbSet<AIInteraction> AiInteractions { get; set; }
    public DbSet<AIContext> AiContext { get; set; }
    public DbSet<AIInsight> AiInsights { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<NucleoTimer>(entity =>
        {
            entity.ToTable("timers"); // Nome da tabela no banco
                
            entity.HasKey(e => e.Id);
                
            entity.Property(e => e.Titulo)
                .HasMaxLength(200);
                
            entity.HasOne(e => e.Nucleo)
                .WithMany()
                .HasForeignKey(e => e.NucleoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ========== USUÁRIOS ==========
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("users_email_key");
            entity.Property(e => e.Phone).HasColumnName("phone").HasMaxLength(20);
            entity.Property(e => e.Cpf).HasColumnName("cpf").HasMaxLength(14);
            entity.HasIndex(e => e.Cpf).IsUnique().HasDatabaseName("users_cpf_key");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.EmailVerified).HasColumnName("email_verified");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasIndex(e => e.UserId).IsUnique().HasDatabaseName("user_profiles_user_id_key");
            entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired().HasMaxLength(200);
            entity.Property(e => e.Nickname).HasColumnName("nickname");
            entity.Property(e => e.AvatarUrl).HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<UserSecurity>(entity =>
        {
            entity.ToTable("user_security");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.HasIndex(e => e.UserId).IsUnique().HasDatabaseName("user_security_user_id_key");
            entity.Property(e => e.LastLogin).HasColumnName("last_login");
            entity.Property(e => e.FailedAttempts).HasColumnName("failed_attempts");
            entity.Property(e => e.PasswordUpdatedAt).HasColumnName("password_updated_at");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        modelBuilder.Entity<UserPreference>(entity =>
        {
            entity.ToTable("user_preferences");
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Theme).HasColumnName("theme");
            entity.Property(e => e.Language).HasColumnName("language");
            entity.Property(e => e.Notifications).HasColumnName("notifications")
                .HasColumnType("jsonb");
            entity.Property(e => e.Shortcuts).HasColumnName("shortcuts")
                .HasColumnType("jsonb");
            entity.Property(e => e.DashboardLayout).HasColumnName("dashboard_layout")
                .HasColumnType("jsonb");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.ToTable("password_resets");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Token).HasColumnName("token");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.Used).HasColumnName("used");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // ========== NÚCLEOS ==========
        modelBuilder.Entity<Nucleo>(entity =>
        {
            entity.ToTable("nucleos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.IconId).HasColumnName("icon_id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.CorDestaque).HasColumnName("cor_destaque");
            entity.Property(e => e.ImagemCapa).HasColumnName("imagem_capa");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.HasOne(e => e.User).WithMany(u => u.Nucleos).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Icon).WithMany(i => i.Nucleos).HasForeignKey(e => e.IconId);
        });

        modelBuilder.Entity<NucleoIcon>(entity =>
        {
            entity.ToTable("nucleo_icons");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.IconUrl).HasColumnName("icon_url");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<NucleoCompartilhamento>(entity =>
        {
            entity.ToTable("nucleo_compartilhamentos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.OwnerUserId).HasColumnName("owner_user_id");
            entity.Property(e => e.SharedWithUserId).HasColumnName("shared_with_user_id");
            entity.Property(e => e.PermissionLevel).HasColumnName("permission_level");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<NucleoRelation>(entity =>
        {
            entity.ToTable("nucleo_relations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SourceNucleoId).HasColumnName("source_nucleo_id");
            entity.Property(e => e.TargetNucleoId).HasColumnName("target_nucleo_id");
            entity.Property(e => e.RelationType).HasColumnName("relation_type");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<NucleoAchievement>(entity =>
        {
            entity.ToTable("nucleo_achievements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.AchievementType).HasColumnName("achievement_type");
            entity.Property(e => e.CurrentValue).HasColumnName("current_value");
            entity.Property(e => e.TargetValue).HasColumnName("target_value");
            entity.Property(e => e.UnlockedAt).HasColumnName("unlocked_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // ========== BLOCOS ==========
        modelBuilder.Entity<Bloco>(entity =>
        {
            entity.ToTable("blocos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Posicao).HasColumnName("posicao");
            entity.Property(e => e.Configuracoes).HasColumnName("configuracoes");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.HasOne(e => e.Nucleo).WithMany(n => n.Blocos).HasForeignKey(e => e.NucleoId);
            entity.Ignore(b => b.CreatedBy); // lembrar remover depois de corrigir a entdidade
            entity.Ignore(b => b.DeletedBy);// lembrar remover depois de corrigir a entdidade
            entity.Ignore(b => b.IsDeleted);// lembrar remover depois de corrigir a entdidade
            entity.Ignore(b => b.UpdatedBy);// lembrar remover depois de corrigir a entdidade
        });

        modelBuilder.Entity<BlocoCalculo>(entity =>
        {
            entity.ToTable("bloco_calculos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
            entity.Property(e => e.TipoOperacao).HasColumnName("tipo_operacao");
            entity.Property(e => e.Campo).HasColumnName("campo");
            entity.Property(e => e.AgruparPor).HasColumnName("agrupar_por");
            entity.Property(e => e.Config).HasColumnName("config");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasIndex(e => e.BlocoId).IsUnique();
            entity.HasOne(e => e.Bloco).WithOne(b => b.Calculo).HasForeignKey<BlocoCalculo>(e => e.BlocoId);
        });

        // ========== LISTAS E ITENS ==========
        modelBuilder.Entity<Lista>(entity =>
        {
            entity.ToTable("listas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
            entity.Property(e => e.TipoLista).HasColumnName("tipo_lista");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.HasOne(e => e.Bloco).WithMany(b => b.Listas).HasForeignKey(e => e.BlocoId);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ListaId).HasColumnName("lista_id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
            entity.Property(e => e.Cor).HasColumnName("cor");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.Lista).WithMany(l => l.Categorias).HasForeignKey(e => e.ListaId);
        });

        modelBuilder.Entity<ItemLista>(entity =>
        {
            entity.ToTable("itens_lista");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ListaId).HasColumnName("lista_id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");
            entity.Property(e => e.ValorUnitario).HasColumnName("valor_unitario");
            entity.Property(e => e.Checked).HasColumnName("checked");
            entity.Property(e => e.ValorTotal).HasColumnName("valor_total");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.HasOne(e => e.Lista).WithMany(l => l.Itens).HasForeignKey(e => e.ListaId);
            entity.HasOne(e => e.Categoria).WithMany(c => c.Itens).HasForeignKey(e => e.CategoriaId);
        });

        // ========== COLEÇÕES DINÂMICAS ==========
        modelBuilder.Entity<Colecao>(entity =>
        {
            entity.ToTable("colecoes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Bloco).WithMany(b => b.Colecoes).HasForeignKey(e => e.BlocoId);
        });

        modelBuilder.Entity<Campo>(entity =>
        {
            entity.ToTable("campos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ColecaoId).HasColumnName("colecao_id");
            entity.Property(e => e.Nome).HasColumnName("nome");
            entity.Property(e => e.TipoCampo).HasColumnName("tipo_campo");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Colecao).WithMany(c => c.Campos).HasForeignKey(e => e.ColecaoId);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("itens");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ColecaoId).HasColumnName("colecao_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Colecao).WithMany(c => c.Itens).HasForeignKey(e => e.ColecaoId);
        });

        modelBuilder.Entity<ItemValor>(entity =>
        {
            entity.ToTable("item_valores");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.CampoId).HasColumnName("campo_id");
            entity.Property(e => e.ValorTexto).HasColumnName("valor_texto");
            entity.Property(e => e.ValorNumerico).HasColumnName("valor_numerico");
            entity.Property(e => e.ValorData).HasColumnName("valor_data");
            entity.Property(e => e.ValorBooleano).HasColumnName("valor_booleano");
            entity.HasOne(e => e.Item).WithMany(i => i.Valores).HasForeignKey(e => e.ItemId);
            entity.HasOne(e => e.Campo).WithMany(c => c.Valores).HasForeignKey(e => e.CampoId);
        });

        // ========== TAREFAS ==========
        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.ToTable("tarefas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo").IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Prioridade).HasColumnName("prioridade");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.DataVencimento).HasColumnName("data_vencimento");
            entity.Property(e => e.ConcluidaEm).HasColumnName("concluida_em");
            entity.Property(e => e.Posicao).HasColumnName("posicao");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.HasOne(e => e.Bloco).WithMany(b => b.Tarefas).HasForeignKey(e => e.BlocoId);
        });

        // ========== HÁBITOS ==========
        modelBuilder.Entity<Habito>(entity =>
        {
            entity.ToTable("habitos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BlocoId).HasColumnName("bloco_id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
            entity.Property(e => e.Frequencia).HasColumnName("frequencia");
            entity.Property(e => e.MetaVezes).HasColumnName("meta_vezes");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Bloco).WithMany(b => b.Habitos).HasForeignKey(e => e.BlocoId);
        });

        modelBuilder.Entity<HabitoRegistro>(entity =>
        {
            entity.ToTable("habitos_registros");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HabitoId).HasColumnName("habito_id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.VezesCompletadas).HasColumnName("vezes_completadas");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.Habito).WithMany(h => h.Registros).HasForeignKey(e => e.HabitoId);
        });

        // ========== CALENDÁRIO ==========
        modelBuilder.Entity<CalendarioEvento>(entity =>
        {
            entity.ToTable("calendario_eventos");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.DataEvento).HasColumnName("data_evento");
            entity.Property(e => e.DuracaoMinutos).HasColumnName("duracao_minutos");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Nucleo).WithMany(n => n.CalendarioEventos).HasForeignKey(e => e.NucleoId);
        });

        // ========== TIMERS ==========
        modelBuilder.Entity<NucleoTimer>(entity =>
        {
            entity.ToTable("timers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Inicio).HasColumnName("inicio");
            entity.Property(e => e.Fim).HasColumnName("fim");
            entity.Property(e => e.DuracaoSegundos).HasColumnName("duracao_segundos");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Nucleo).WithMany(n => n.NucleoTimers).HasForeignKey(e => e.NucleoId);
        });

        // ========== METAS ==========
        modelBuilder.Entity<Meta>(entity =>
        {
            entity.ToTable("metas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo").IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.ValorMeta).HasColumnName("valor_meta");
            entity.Property(e => e.ValorAtual).HasColumnName("valor_atual");
            entity.Property(e => e.DataInicio).HasColumnName("data_inicio");
            entity.Property(e => e.DataFim).HasColumnName("data_fim");
            entity.Property(e => e.Concluida).HasColumnName("concluida");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.Nucleo).WithMany(n => n.Metas).HasForeignKey(e => e.NucleoId);
        });

        // ========== GAMIFICAÇÃO ==========
        modelBuilder.Entity<Conquista>(entity =>
        {
            entity.ToTable("conquistas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome).HasColumnName("nome").IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("descricao");
            entity.Property(e => e.IconeUrl).HasColumnName("icone_url");
            entity.Property(e => e.Tipo).HasColumnName("tipo");
            entity.Property(e => e.Condicao).HasColumnName("condicao");
            entity.Property(e => e.XpRecompensa).HasColumnName("xp_recompensa");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<UserConquista>(entity =>
        {
            entity.ToTable("user_conquistas");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ConquistaId).HasColumnName("conquista_id");
            entity.Property(e => e.DesbloqueadoEm).HasColumnName("desbloqueado_em");
            entity.HasOne(e => e.User).WithMany(u => u.Conquistas).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Conquista).WithMany(c => c.UserConquistas).HasForeignKey(e => e.ConquistaId);
        });

        modelBuilder.Entity<UserLevel>(entity =>
        {
            entity.ToTable("user_levels");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.CurrentXp).HasColumnName("current_xp");
            entity.Property(e => e.NextLevelXp).HasColumnName("next_level_xp");
            entity.Property(e => e.TotalXpEarned).HasColumnName("total_xp_earned");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.User).WithOne(u => u.Level).HasForeignKey<UserLevel>(e => e.UserId);
        });

        modelBuilder.Entity<Streak>(entity =>
        {
            entity.ToTable("streaks");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.StreakType).HasColumnName("streak_type");
            entity.Property(e => e.CurrentStreak).HasColumnName("current_streak");
            entity.Property(e => e.MaxStreak).HasColumnName("max_streak");
            entity.Property(e => e.LastActivityDate).HasColumnName("last_activity_date");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasOne(e => e.User).WithMany(u => u.Streaks).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Nucleo).WithMany().HasForeignKey(e => e.NucleoId);
        });

        modelBuilder.Entity<XP_Log>(entity =>
        {
            entity.ToTable("xp_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.XpAmount).HasColumnName("xp_amount");
            entity.Property(e => e.Source).HasColumnName("source");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany(u => u.XpLogs).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Nucleo).WithMany().HasForeignKey(e => e.NucleoId);
        });

        modelBuilder.Entity<EnergyLog>(entity =>
        {
            entity.ToTable("energy_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.EnergyAmount).HasColumnName("energy_amount");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany(u => u.EnergyLogs).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Nucleo).WithMany().HasForeignKey(e => e.NucleoId);
        });

        // ========== NOTIFICAÇÕES ==========
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Titulo).HasColumnName("titulo");
            entity.Property(e => e.Mensagem).HasColumnName("mensagem");
            entity.Property(e => e.Read).HasColumnName("read");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany(u => u.Notifications).HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.ToTable("activity_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Acao).HasColumnName("acao");
            entity.Property(e => e.Metadata).HasColumnName("metadata");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany(u => u.ActivityLogs).HasForeignKey(e => e.UserId);
        });

        // ========== ASSINATURAS ==========
        modelBuilder.Entity<Plan>(entity =>
        {
            entity.ToTable("plans");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
            entity.Property(e => e.MaxNucleos).HasColumnName("max_nucleos");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.ToTable("subscriptions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity.Property(e => e.StartedAt).HasColumnName("started_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.HasOne(e => e.User).WithOne(u => u.Subscription).HasForeignKey<Subscription>(e => e.UserId);
            entity.HasOne(e => e.Plan).WithMany(p => p.Subscriptions).HasForeignKey(e => e.PlanId);
        });

        // ========== IA ==========
        modelBuilder.Entity<AIInteraction>(entity =>
        {
            entity.ToTable("ai_interactions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Mensagem).HasColumnName("mensagem");
            entity.Property(e => e.Resposta).HasColumnName("resposta");
            entity.Property(e => e.Contexto).HasColumnName("contexto");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany(u => u.AiInteractions).HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<AIContext>(entity =>
        {
            entity.ToTable("ai_context");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LastSummary).HasColumnName("last_summary");
            entity.Property(e => e.PreferredStyle).HasColumnName("preferred_style");
            entity.Property(e => e.LastInteraction).HasColumnName("last_interaction");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.HasIndex(e => e.UserId).IsUnique();
            entity.HasOne(e => e.User).WithOne(u => u.AiContext).HasForeignKey<AIContext>(e => e.UserId);
        });

        modelBuilder.Entity<AIInsight>(entity =>
        {
            entity.ToTable("ai_insights");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.NucleoId).HasColumnName("nucleo_id");
            entity.Property(e => e.InsightType).HasColumnName("insight_type");
            entity.Property(e => e.Mensagem).HasColumnName("mensagem");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.Applied).HasColumnName("applied");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Nucleo).WithMany().HasForeignKey(e => e.NucleoId);
        });
    }
}
