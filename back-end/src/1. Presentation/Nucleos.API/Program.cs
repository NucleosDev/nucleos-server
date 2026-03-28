using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Nucleos.Application;
using Nucleos.Infrastructure;
using Nucleos.Infrastructure.Persistence.Context;
using Nucleos.API.Scripts;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CARREGAR .ENV PRIMEIRO
// ============================================
Env.Load();

// ============================================
// LER VARIÁVEIS DIRETAMENTE DO AMBIENTE
// ============================================
// Supabase
var supabaseHost = Environment.GetEnvironmentVariable("SUPABASE_HOST") ?? "";
var supabasePort = Environment.GetEnvironmentVariable("SUPABASE_PORT") ?? "";
var supabaseDatabase = Environment.GetEnvironmentVariable("SUPABASE_DATABASE") ?? "";
var supabaseUsername = Environment.GetEnvironmentVariable("SUPABASE_USERNAME") ?? "";
var supabasePassword = Environment.GetEnvironmentVariable("SUPABASE_PASSWORD") ?? "";

// JWT
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? 
             "Nucleos-Super-Secret-Key-Min-32-Characters-Long-For-JWT!";
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "https://localhost:5000";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "https://localhost:5000";
var jwtExpiresMinutesStr = Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60";

// Log para debug
Console.WriteLine($"🔑 JWT_KEY loaded: {jwtKey.Substring(0, Math.Min(10, jwtKey.Length))}... (length: {jwtKey.Length})");
Console.WriteLine($"⏱️ JWT_EXPIRES_MINUTES: {jwtExpiresMinutesStr}");

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Adicionar serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// ADICIONAR OS SERVICOS DA APLICAÇÃO E INFRAESTRUTURA
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// ============================================
// CONFIGURAR CONEXÃO COM SUPABASE
// ============================================
var connectionString = $"Host={supabaseHost};Port={supabasePort};Database={supabaseDatabase};Username={supabaseUsername};Password={supabasePassword};SSL Mode=Require;Trust Server Certificate=true";

// Log (sem senha)
var logConnection = connectionString.Replace(supabasePassword, "***");
Console.WriteLine($"Connection String: {logConnection}");

builder.Services.AddDbContext<NucleosDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(true);
});

// ============================================
// REGISTRAR JWT SERVICES
// ============================================
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

// ============================================
// CONFIGURAR AUTENTICAÇÃO JWT
// ============================================
// Validar tamanho da chave
if (jwtKey.Length < 32)
{
    Console.WriteLine($"⚠️ JWT_KEY tem apenas {jwtKey.Length} caracteres. Recomendado mínimo de 32 caracteres.");
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

// ============================================
// TESTAR CONEXÃO E EXECUTAR SEED
// ============================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NucleosDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("TESTANDO CONEXÃO COM O BANCO DE DADOS (SUPABASE)");
    
    try
    {
        var canConnect = await dbContext.Database.CanConnectAsync();
        
        if (canConnect)
        {
            logger.LogInformation("✅ Conexão com Supabase estabelecida com sucesso!");
            
            // Executar seed apenas em desenvolvimento
            if (app.Environment.IsDevelopment())
            {
                logger.LogInformation("🌱 Ambiente de desenvolvimento - Executando seed...");
                await SeedData.RunAsync(scope.ServiceProvider);
            }
            
            var userCount = await dbContext.Users.CountAsync();
            logger.LogInformation($"👥 Total de usuários na base: {userCount}");
        }
        else
        {
            logger.LogError("❌ Não foi possível conectar ao Supabase");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Erro ao conectar ao Supabase");
        logger.LogError($"Mensagem: {ex.Message}");
    }
}

// ============================================
// CONFIGURAR PIPELINE
// ============================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));
app.MapGet("/", () => Results.Ok(new { name = "Nucleos API", version = "1.0.0", status = "running" }));

app.Run();