using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nucleos.API.Scripts;
using Nucleos.Application;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Infrastructure;
using Nucleos.Infrastructure.Identity;
using Nucleos.Infrastructure.Persistence.Context;
using Nucleos.API.Middleware;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Loopback, 5000, listenOptions =>
    {
        listenOptions.UseHttps(); // usa o certificado de desenvolvimento
    });
});

// Carregar .env
Env.Load();

// Ler variáveis de ambiente
var supabaseHost = Environment.GetEnvironmentVariable("SUPABASE_HOST") ?? "";
var supabasePort = Environment.GetEnvironmentVariable("SUPABASE_PORT") ?? "";
var supabaseDatabase = Environment.GetEnvironmentVariable("SUPABASE_DATABASE") ?? "";
var supabaseUsername = Environment.GetEnvironmentVariable("SUPABASE_USERNAME") ?? "";
var supabasePassword = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD") ?? "";

var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ??
             "Nucleos-Super-Secret-Key-Min-32-Characters-Long-For-JWT!";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "https://localhost:5000";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "https://localhost:5000";
var jwtExpiresMinutesStr = Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60";

Console.WriteLine($"JWT_KEY loaded: {jwtKey.Substring(0, Math.Min(10, jwtKey.Length))}... (length: {jwtKey.Length})");
Console.WriteLine($"JWT_EXPIRES_MINUTES: {jwtExpiresMinutesStr}");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nucleos API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// Configuração CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "https://nucleos-ui.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Adicionar serviços da aplicação e infraestrutura
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Conexão com Supabase
var connectionString = $"Host={supabaseHost};Port={supabasePort};Database={supabaseDatabase};Username={supabaseUsername};Password={supabasePassword};SSL Mode=Require;Trust Server Certificate=true";

var logConnection = connectionString.Replace(supabasePassword, "***");
Console.WriteLine($"Connection String: {logConnection}");

builder.Services.AddDbContext<NucleosDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(true);
});

// JWT services
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

if (jwtKey.Length < 32)
{
    Console.WriteLine($"Aviso: JWT_KEY tem apenas {jwtKey.Length} caracteres. Recomendado mínimo de 32 caracteres.");
}

var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // 🔥 LER TOKEN DO COOKIE
            var token = context.Request.Cookies["auth_token"];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware pipeline
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Endpoints adicionais
app.MapGet("/api/endpoints", (IActionDescriptorCollectionProvider provider) =>
{
    var endpoints = provider.ActionDescriptors.Items
        .OfType<ControllerActionDescriptor>()
        .Select(desc => new
        {
            Controller = desc.ControllerName,
            Action = desc.ActionName,
            Route = desc.AttributeRouteInfo?.Template ?? "",
            HttpMethod = desc.ActionConstraints?.OfType<HttpMethodActionConstraint>()
                            .FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET",
            Area = desc.RouteValues.ContainsKey("area") ? desc.RouteValues["area"] : null
        })
        .Where(e => !string.IsNullOrEmpty(e.Route))
        .OrderBy(e => e.Controller)
        .ThenBy(e => e.Action)
        .ToList();

    return Results.Ok(endpoints);
});

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
app.MapGet("/", () => Results.Ok(new { name = "Nucleos API", version = "1.0.0", status = "running" }));

// Teste de conexão com o banco (antes de iniciar a aplicação)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NucleosDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("Testando conexão com o banco de dados (Supabase)");
    
    try
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("Conexão com Supabase estabelecida com sucesso!");
            
            if (app.Environment.IsDevelopment())
            {
                logger.LogInformation("Ambiente de desenvolvimento - Executando seed...");
                // await SeedData.RunAsync(scope.ServiceProvider);
            }
            
            var userCount = await dbContext.Users.CountAsync();
            logger.LogInformation($"Total de usuários na base: {userCount}");
        }
        else
        {
            logger.LogError("Não foi possível conectar ao Supabase");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao conectar ao Supabase");
        logger.LogError($"Mensagem: {ex.Message}");
    }
}

app.Run();