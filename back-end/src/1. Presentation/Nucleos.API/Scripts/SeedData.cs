using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nucleos.Infrastructure.Persistence.Context;
using Nucleos.Domain.Entities;
using BCrypt.Net;
using Microsoft.Extensions.Logging;

namespace Nucleos.API.Scripts;

public class SeedData
{
    public static async Task RunAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<NucleosDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedData>>();

        logger.LogInformation("🌱 Iniciando seed completo do banco de dados...");

        try
        {
            // ============================================
            // 1. USUÁRIOS
            // ============================================
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("📝 Criando usuários...");

                var users = new List<User>
                {
                    new User { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Email = "admin@nucleos.com", Phone = "(11) 99999-0001", Cpf = "12345678901", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123", 12), EmailVerified = true, Active = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new User { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Email = "joao.silva@email.com", Phone = "(11) 99999-0002", Cpf = "23456789012", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Joao@123", 12), EmailVerified = true, Active = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new User { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Email = "maria.souza@email.com", Phone = "(11) 99999-0003", Cpf = "34567890123", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Maria@123", 12), EmailVerified = true, Active = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new User { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Email = "pedro.oliveira@email.com", Phone = "(11) 99999-0004", Cpf = "45678901234", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Pedro@123", 12), EmailVerified = true, Active = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new User { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Email = "ana.costa@email.com", Phone = "(11) 99999-0005", Cpf = "56789012345", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ana@123", 12), EmailVerified = true, Active = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                await context.Users.AddRangeAsync(users);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {users.Count} usuários criados");
            }

            // ============================================
            // 2. PERFIS
            // ============================================
            if (!await context.UserProfiles.AnyAsync())
            {
                logger.LogInformation("📝 Criando perfis...");
                var profiles = new List<UserProfile>
                {
                    new UserProfile { Id = Guid.NewGuid(), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), FullName = "Administrador do Sistema", Nickname = "Admin", AvatarUrl = "https://ui-avatars.com/api/?background=667eea&color=fff&name=Admin", CreatedAt = DateTime.UtcNow },
                    new UserProfile { Id = Guid.NewGuid(), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), FullName = "João Silva", Nickname = "João", AvatarUrl = "https://ui-avatars.com/api/?background=10b981&color=fff&name=João", CreatedAt = DateTime.UtcNow },
                    new UserProfile { Id = Guid.NewGuid(), UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), FullName = "Maria Souza", Nickname = "Maria", AvatarUrl = "https://ui-avatars.com/api/?background=f59e0b&color=fff&name=Maria", CreatedAt = DateTime.UtcNow },
                    new UserProfile { Id = Guid.NewGuid(), UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), FullName = "Pedro Oliveira", Nickname = "Pedro", AvatarUrl = "https://ui-avatars.com/api/?background=ef4444&color=fff&name=Pedro", CreatedAt = DateTime.UtcNow },
                    new UserProfile { Id = Guid.NewGuid(), UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"), FullName = "Ana Costa", Nickname = "Ana", AvatarUrl = "https://ui-avatars.com/api/?background=8b5cf6&color=fff&name=Ana", CreatedAt = DateTime.UtcNow }
                };
                await context.UserProfiles.AddRangeAsync(profiles);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {profiles.Count} perfis criados");
            }

            // ============================================
            // 3. ROLES
            // ============================================
            if (!await context.UserRoles.AnyAsync())
            {
                logger.LogInformation("📝 Criando roles...");
                var roles = new List<UserRole>
                {
                    new UserRole { Id = Guid.NewGuid(), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Role = "admin" },
                    new UserRole { Id = Guid.NewGuid(), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Role = "user" },
                    new UserRole { Id = Guid.NewGuid(), UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Role = "user" },
                    new UserRole { Id = Guid.NewGuid(), UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Role = "user" },
                    new UserRole { Id = Guid.NewGuid(), UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Role = "user" }
                };
                await context.UserRoles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {roles.Count} roles criadas");
            }

            // ============================================
            // 4. SEGURANÇA
            // ============================================
            if (!await context.UserSecurity.AnyAsync())
            {
                logger.LogInformation("📝 Criando registros de segurança...");
                var security = new List<UserSecurity>
                {
                    new UserSecurity { Id = Guid.NewGuid(), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), FailedAttempts = 0, PasswordUpdatedAt = DateTime.UtcNow, LastLogin = DateTime.UtcNow },
                    new UserSecurity { Id = Guid.NewGuid(), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), FailedAttempts = 0, PasswordUpdatedAt = DateTime.UtcNow },
                    new UserSecurity { Id = Guid.NewGuid(), UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), FailedAttempts = 0, PasswordUpdatedAt = DateTime.UtcNow },
                    new UserSecurity { Id = Guid.NewGuid(), UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), FailedAttempts = 0, PasswordUpdatedAt = DateTime.UtcNow },
                    new UserSecurity { Id = Guid.NewGuid(), UserId = Guid.Parse("55555555-5555-5555-5555-555555555555"), FailedAttempts = 0, PasswordUpdatedAt = DateTime.UtcNow }
                };
                await context.UserSecurity.AddRangeAsync(security);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {security.Count} registros de segurança criados");
            }

            // ============================================
            // 5. PREFERÊNCIAS
            // ============================================
            if (!await context.UserPreferences.AnyAsync())
            {
                logger.LogInformation("📝 Criando preferências...");
                var preferences = new List<UserPreference>
                {
                    new UserPreference { UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Theme = "dark", Language = "pt-BR", Notifications = "{\"push\":true,\"email\":true,\"streaks\":true}", Shortcuts = "{}", DashboardLayout = "{\"layout\":\"compact\"}", UpdatedAt = DateTime.UtcNow },
                    new UserPreference { UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Theme = "light", Language = "pt-BR", Notifications = "{\"push\":true,\"email\":true,\"streaks\":true}", Shortcuts = "{}", DashboardLayout = "{}", UpdatedAt = DateTime.UtcNow },
                    new UserPreference { UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Theme = "system", Language = "pt-BR", Notifications = "{\"push\":true,\"email\":true,\"streaks\":true}", Shortcuts = "{}", DashboardLayout = "{}", UpdatedAt = DateTime.UtcNow }
                };
                await context.UserPreferences.AddRangeAsync(preferences);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {preferences.Count} preferências criadas");
            }

            // ============================================
            // 6. PLANOS
            // ============================================
            if (!await context.Plans.AnyAsync())
            {
                logger.LogInformation("📝 Criando planos...");
                var plans = new List<Plan>
                {
                    new Plan { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Free", MaxNucleos = 3, Price = 0, CreatedAt = DateTime.UtcNow },
                    new Plan { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Pro", MaxNucleos = 10, Price = 29.90m, CreatedAt = DateTime.UtcNow },
                    new Plan { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Enterprise", MaxNucleos = 999, Price = 99.90m, CreatedAt = DateTime.UtcNow }
                };
                await context.Plans.AddRangeAsync(plans);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {plans.Count} planos criados");
            }

            // ============================================
            // 7. ASSINATURAS (CORRIGIDO)
            // ============================================
            if (!await context.Subscriptions.AnyAsync())
            {
                logger.LogInformation("📝 Criando assinaturas...");
                
                // Buscar os planos do banco
                var plans = await context.Plans.ToListAsync();
                var freePlan = plans.FirstOrDefault(p => p.Name == "Free");
                
                if (freePlan == null)
                {
                    logger.LogError("❌ Plano Free não encontrado!");
                    throw new Exception("Plano Free não encontrado no banco de dados");
                }
                
                var subscriptions = new List<Subscription>
                {
                    new Subscription { Id = Guid.NewGuid(), UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"), PlanId = freePlan.Id, StartedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddYears(1) },
                    new Subscription { Id = Guid.NewGuid(), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), PlanId = freePlan.Id, StartedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddMonths(1) },
                    new Subscription { Id = Guid.NewGuid(), UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), PlanId = freePlan.Id, StartedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddMonths(1) }
                };
                await context.Subscriptions.AddRangeAsync(subscriptions);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {subscriptions.Count} assinaturas criadas");
            }

            // ============================================
            // 8. NÚCLEOS
            // ============================================
            if (!await context.Nucleos.AnyAsync())
            {
                logger.LogInformation("📝 Criando núcleos...");
                var nucleos = new List<Nucleo>
                {
                    new Nucleo { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nome = "Desenvolvimento Pessoal", Descricao = "Crescimento pessoal e profissional", CorDestaque = "#10B981", ImagemCapa = "https://picsum.photos/id/1/300/200", Tipo = "pessoal", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Nucleo { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nome = "Trabalho", Descricao = "Projetos profissionais", CorDestaque = "#3B82F6", ImagemCapa = "https://picsum.photos/id/20/300/200", Tipo = "profissional", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Nucleo { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), UserId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Nome = "Saúde", Descricao = "Hábitos saudáveis", CorDestaque = "#EF4444", ImagemCapa = "https://picsum.photos/id/29/300/200", Tipo = "saude", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                    new Nucleo { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), UserId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Nome = "Estudos", Descricao = "Cursos e aprendizados", CorDestaque = "#F59E0B", ImagemCapa = "https://picsum.photos/id/0/300/200", Tipo = "estudo", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                };
                await context.Nucleos.AddRangeAsync(nucleos);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {nucleos.Count} núcleos criados");
            }

            // ============================================
            // 9. BLOCOS
            // ============================================
            if (!await context.Blocos.AnyAsync())
            {
                logger.LogInformation("📝 Criando blocos...");
                var blocos = new List<Bloco>();
                var nucleos = await context.Nucleos.ToListAsync();

                foreach (var nucleo in nucleos)
                {
                    blocos.Add(new Bloco { Id = Guid.NewGuid(), NucleoId = nucleo.Id, Tipo = "lista", Titulo = "Tarefas Pendentes", Posicao = 1, Configuracoes = "{}", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    blocos.Add(new Bloco { Id = Guid.NewGuid(), NucleoId = nucleo.Id, Tipo = "habito", Titulo = "Hábitos Diários", Posicao = 2, Configuracoes = "{}", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    blocos.Add(new Bloco { Id = Guid.NewGuid(), NucleoId = nucleo.Id, Tipo = "calendario", Titulo = "Eventos", Posicao = 3, Configuracoes = "{}", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    blocos.Add(new Bloco { Id = Guid.NewGuid(), NucleoId = nucleo.Id, Tipo = "NucleoTimer", Titulo = "Pomodoro", Posicao = 4, Configuracoes = "{}", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
                await context.Blocos.AddRangeAsync(blocos);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {blocos.Count} blocos criados");
            }

            // ============================================
            // 10. LISTAS
            // ============================================
            if (!await context.Listas.AnyAsync())
            {
                logger.LogInformation("📝 Criando listas...");
                var listas = new List<Lista>();
                var blocos = await context.Blocos.Where(b => b.Tipo == "lista").ToListAsync();

                foreach (var bloco in blocos)
                {
                    listas.Add(new Lista { Id = Guid.NewGuid(), BlocoId = bloco.Id, Nome = "Tarefas Importantes", TipoLista = "generica", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    listas.Add(new Lista { Id = Guid.NewGuid(), BlocoId = bloco.Id, Nome = "Lista de Compras", TipoLista = "compras", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                    listas.Add(new Lista { Id = Guid.NewGuid(), BlocoId = bloco.Id, Nome = "Despesas do Mês", TipoLista = "financeiro", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
                await context.Listas.AddRangeAsync(listas);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {listas.Count} listas criadas");
            }

            // ============================================
            // 11. TIMERS (NUCLEOTIMERS)
            // ============================================
            if (!await context.NucleoTimers.AnyAsync())
            {
                logger.LogInformation("📝 Criando timers...");
                var timers = new List<NucleoTimer>();
                var nucleos = await context.Nucleos.ToListAsync();

                foreach (var nucleo in nucleos)
                {
                    timers.Add(new NucleoTimer 
                    { 
                        Id = Guid.NewGuid(), 
                        NucleoId = nucleo.Id, 
                        Titulo = "Estudo Pomodoro", 
                        Inicio = DateTime.UtcNow.AddHours(-1), 
                        Fim = DateTime.UtcNow, 
                        DuracaoSegundos = 3600, 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    });
                    timers.Add(new NucleoTimer 
                    { 
                        Id = Guid.NewGuid(), 
                        NucleoId = nucleo.Id, 
                        Titulo = "Exercícios", 
                        Inicio = DateTime.UtcNow.AddHours(-2), 
                        Fim = DateTime.UtcNow.AddHours(-1), 
                        DuracaoSegundos = 1800, 
                        CreatedAt = DateTime.UtcNow, 
                        UpdatedAt = DateTime.UtcNow 
                    });
                }
                await context.NucleoTimers.AddRangeAsync(timers);
                await context.SaveChangesAsync();
                logger.LogInformation($"✅ {timers.Count} timers criados");
            }

            await context.SaveChangesAsync();
            
            // Resumo final
            logger.LogInformation("🎉 Seed completo finalizado com sucesso!");
            logger.LogInformation($"📊 Resumo: {await context.Users.CountAsync()} usuários, {await context.Nucleos.CountAsync()} núcleos");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "❌ Erro durante o seed");
            throw;
        }
    }
}