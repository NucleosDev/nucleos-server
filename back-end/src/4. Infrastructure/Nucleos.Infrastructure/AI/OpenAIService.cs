using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.AI;

public class OpenAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<OpenAIService> _logger;

    public OpenAIService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenAIService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["OpenAI:ApiKey"];
        _logger = logger;
    }

    public Task<string> GerarInsightAsync(string contexto, CancellationToken ct = default)
        => GenerateInsightAsync(contexto, ct);

    public Task<string> EnviarMensagemAsync(string mensagem, string? historico = null, CancellationToken ct = default)
        => ChatAsync(mensagem, historico, ct);

    private async Task<string> GenerateInsightAsync(string context, CancellationToken ct)
    {
        try
        {
            var payload = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "Você é um assistente que gera insights de produtividade." },
                    new { role = "user", content = context }
                },
                max_tokens = 150,
                temperature = 0.7
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<OpenAIResponse>(json);

            return result?.Choices?[0]?.Message?.Content ?? "Não foi possível gerar insight.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar OpenAI");
            return "Desculpe, houve um erro ao gerar o insight.";
        }
    }

    private async Task<string> ChatAsync(string userMessage, string? historico, CancellationToken ct)
    {
        await Task.CompletedTask;
        return string.IsNullOrEmpty(historico)
            ? $"Resposta mock para: {userMessage}"
            : $"Resposta mock (com histórico) para: {userMessage}";
    }
}

public class OpenAIResponse
{
    public Choice[] Choices { get; set; }
}

public class Choice
{
    public Message Message { get; set; }
}

public class Message
{
    public string Content { get; set; }
}