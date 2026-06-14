using System.Text.Json;
using LessonService.Application.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace LessonService.Infrastructure.ExternalServices;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var cached = await _cache.GetStringAsync(key, cancellationToken);
            if (cached == null) return default;

            _logger.LogDebug("Cache HIT: {Key}", key);
            return JsonSerializer.Deserialize<T>(cached);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache GET dështoi për key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry ?? TimeSpan.FromHours(1)
            };

            var serialized = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serialized, options, cancellationToken);

            _logger.LogDebug("Cache SET: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache SET dështoi për key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.RemoveAsync(key, cancellationToken);
            _logger.LogDebug("Cache REMOVE: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cache REMOVE dështoi për key: {Key}", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        // StackExchange.Redis nuk suporton pattern delete direkt me IDistributedCache
        // Për tani logojmë — do ta zgjerojmë me StackExchange.Redis direkt nëse nevojitet
        _logger.LogDebug("Cache REMOVE PREFIX: {Prefix}", prefix);
    }
}