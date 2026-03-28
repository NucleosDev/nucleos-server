using System.Collections.Concurrent;

namespace Nucleos.API.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int count, DateTime window)> _requests = new();
    private const int MaxRequests = 100;
    private static readonly TimeSpan Window = TimeSpan.FromMinutes(1);

    public RateLimitingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var now = DateTime.UtcNow;
        var entry = _requests.GetOrAdd(ip, _ => (0, now));
        if (now - entry.window > Window) entry = (0, now);
        entry = (entry.count + 1, entry.window);
        _requests[ip] = entry;
        if (entry.count > MaxRequests) { context.Response.StatusCode = 429; await context.Response.WriteAsync("Too many requests"); return; }
        await _next(context);
    }
}
