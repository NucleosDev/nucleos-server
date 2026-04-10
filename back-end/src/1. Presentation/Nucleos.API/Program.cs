using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nucleos.API.Middleware;
using Nucleos.Application;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Infrastructure;
using Nucleos.Infrastructure.Identity;
using Nucleos.Infrastructure.Persistence.Context;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 🔥 LOAD ENV
Env.Load();
builder.Configuration.AddEnvironmentVariables();

// 🔥 ENV VARS
string GetEnv(string key)
{
    var value = builder.Configuration[key];
    if (string.IsNullOrWhiteSpace(value))
        throw new Exception($"{key} não configurado");
    return value;
}

var supabaseHost = GetEnv("SUPABASE_HOST");
var supabasePort = GetEnv("SUPABASE_PORT");
var supabaseDatabase = GetEnv("SUPABASE_DATABASE");
var supabaseUsername = GetEnv("SUPABASE_USERNAME");
var supabasePassword = GetEnv("SUPABASE_PASSWORD");

var jwtKey = GetEnv("JWT_KEY");
var jwtIssuer = GetEnv("JWT_ISSUER");
var jwtAudience = GetEnv("JWT_AUDIENCE");

// 🔐 LOG SAFE
Console.WriteLine($"JWT_KEY length: {jwtKey.Length}");
Console.WriteLine($"JWT_KEY (API): {jwtKey}");

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 🔥 CONFIGURAÇÃO DE JSON (CASE-INSENSITIVE)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Aceita tanto PascalCase quanto camelCase nas requisições
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        
        // Respostas em camelCase (padrão JavaScript)
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        
        // Configurações adicionais
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();

// 🔥 SWAGGER + JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nucleos API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
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
});

// 🔥 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
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

// 🔥 SERVICES
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// 🔥 DB
var connectionString =
    $"Host={supabaseHost};Port={supabasePort};Database={supabaseDatabase};Username={supabaseUsername};Password={supabasePassword};SSL Mode=Require;Trust Server Certificate=true";

builder.Services.AddDbContext<NucleosDbContext>(options =>
{
    options.UseNpgsql(connectionString);

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// 🔥 JWT
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

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
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ JWT ERROR: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("✅ TOKEN VALIDADO");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine("⚠️ CHALLENGE: Token inválido ou ausente");
            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,

        ValidateIssuer = false,
        ValidIssuer = jwtIssuer,

        ValidateAudience = false,
        ValidAudience = jwtAudience,

        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,

        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

// 🔥 AUTO MAPPER
builder.Services.AddAutoMapper(typeof(Nucleos.Application.Common.Mappings.MappingProfile));
builder.Services.AddAuthorization();

var app = builder.Build();

// 🔥 PIPELINE
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🔥 HEALTH
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();