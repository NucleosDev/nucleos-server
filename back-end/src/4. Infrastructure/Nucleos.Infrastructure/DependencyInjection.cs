using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Application.Features.Calculos.Services;
using Nucleos.Application.Features.Gamificacao.Services;
using Nucleos.Domain.Interfaces;
using Nucleos.Infrastructure.AI;
using Nucleos.Infrastructure.Calculo;
using Nucleos.Infrastructure.Gamification;
using Nucleos.Infrastructure.Identity;
using Nucleos.Infrastructure.Persistence.Context;
using Nucleos.Infrastructure.Persistence.Repositories;
using Nucleos.Infrastructure.Persistence.UnitOfWork;
using Nucleos.Infrastructure.Services.Cache;
using Nucleos.Infrastructure.Services.DateTime;

namespace Nucleos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // HttpContextAccessor (necessário para CurrentUserService)
        services.AddHttpContextAccessor();

        // DbContext
        services.AddDbContext<NucleosDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Supabase")));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped(typeof(Nucleos.Application.Common.Interfaces.IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INucleoRepository, NucleoRepository>();
        services.AddScoped<IBlocoRepository, BlocoRepository>();

        // Application DbContext interface
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<NucleosDbContext>());

        // Identity
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Services
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();

        // Gamification
        services.AddScoped<Nucleos.Application.Features.Gamificacao.Services.IGamificationEngine, GamificationEngine>();
        services.AddScoped<IConquistaChecker, ConquistaChecker>();

        // AI
        services.AddHttpClient<IAIService, OpenAIService>();

        // Cálculo
        services.AddScoped<ICalculoEngine, CalculoEngine>();

        // Notificações
        services.AddScoped<Application.Common.Interfaces.INotificationService, Services.Notifications.NotificationService>();

        // Email
        services.AddScoped<Application.Common.Interfaces.IEmailService, Services.Email.EmailService>();

        // File Storage
        services.AddScoped<Application.Common.Interfaces.IFileStorageService, Services.FileStorage.LocalFileStorageService>();

        return services;
    }
}
