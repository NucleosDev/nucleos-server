namespace Nucleos.Infrastructure.AI;
public static class PromptTemplates
{
    public static string GerarInsight(string ctx) =>
        $"Você é assistente de produtividade. Gere um insight útil e acionável.\nContexto: {ctx}\nInsight:";
    public static string Chat(string msg, string? hist = null) =>
        hist != null ? $"Histórico:\n{hist}\nUsuário: {msg}\nAssistente:" : $"Usuário: {msg}\nAssistente:";
}
