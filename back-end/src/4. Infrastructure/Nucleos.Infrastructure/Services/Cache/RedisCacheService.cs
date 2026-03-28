using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Nucleos.Application.Common.Interfaces;

namespace Nucleos.Infrastructure.Services.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    public RedisCacheService(IDistributedCache cache) => _cache = cache;

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
    {
        var data = await _cache.GetStringAsync(key, ct);
        return data == null ? default : JsonSerializer.Deserialize<T>(data);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken ct = default)
    {
        var opts = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromMinutes(30) };
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), opts, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default) => await _cache.RemoveAsync(key, ct);
}
