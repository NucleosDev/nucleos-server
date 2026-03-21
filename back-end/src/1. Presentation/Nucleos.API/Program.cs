using Microsoft.EntityFrameworkCore;
using Nucleos.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Carregar configurações
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Adicionar serviços
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// ============================================
// CONFIGURAR CONEXÃO COM SUPABASE
// ============================================
var connectionString = builder.Configuration.GetConnectionString("Supabase");

// Substituir placeholder se necessário
if (connectionString?.Contains("{Supabase:Password}") == true)
{
    var password = builder.Configuration["Supabase:Password"];
    if (!string.IsNullOrEmpty(password))
    {
        connectionString = connectionString.Replace("{Supabase:Password}", password);
    }
}

builder.Services.AddDbContext<NucleosDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

var app = builder.Build();

// ============================================
// TESTAR CONEXÃO COM O BANCO
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
            logger.LogInformation("Conexão com Supabase estabelecida com sucesso!");
            
            // Contar usuários
            var userCount = await dbContext.Users.CountAsync();
            logger.LogInformation($"Total de usuários na base: {userCount}");
        }
        else
        {
            logger.LogError("Não foi possível conectar ao Supabase");
            logger.LogError("   Verifique a senha no User Secrets: dotnet user-secrets set \"Supabase:Password\" \"sua_senha\"");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao conectar ao Supabase");
        logger.LogError($"   Detalhes: {ex.Message}");
        
        if (ex.Message.Contains("password"))
            logger.LogError("Senha incorreta. Verifique no Supabase Dashboard");
        if (ex.Message.Contains("host"))
            logger.LogError("Host incorreto. Use o pooler: aws-0-us-west-1.pooler.supabase.com");
        if (ex.Message.Contains("timeout"))
            logger.LogError("Timeout. Verifique se o IP está liberado no Supabase");
    }
    
    logger.LogInformation("════════════════════════════════════════════════════════════");
}

// Configurar pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();

// Health check
app.MapGet("/health", () => Results.Ok(new 
{ 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    message = "Nucleos API is running!"
}));

// Home
app.MapGet("/", () => Results.Ok(new
{
    name = "Nucleos API",
    version = "1.0.0",
    status = "running",
    database = "Supabase (PostgreSQL)"
}));

app.Run();