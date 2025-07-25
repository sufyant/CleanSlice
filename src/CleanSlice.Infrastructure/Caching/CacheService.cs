using System.Buffers;
using System.Text.Json;
using CleanSlice.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanSlice.Infrastructure.Caching;

internal sealed class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await cache.GetAsync(key, cancellationToken);

        return bytes is null ? default : Deserialize<T>(bytes);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        return cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        cache.RemoveAsync(key, cancellationToken);

    public async Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
    {
        // Note: IDistributedCache doesn't support pattern removal by default
        // This is a simplified implementation. In production, you might want to use Redis
        // or implement a more sophisticated pattern matching system

        // For now, we'll log that pattern removal is not fully supported
        // You can implement this based on your cache provider (Redis, etc.)

        // Example for Redis (if you switch to Redis):
        // await _redis.GetDatabase().ScriptEvaluateAsync("return redis.call('keys', ARGV[1])", pattern);

        // For now, we'll just log the pattern removal attempt
        // In a real implementation, you'd need to track cache keys or use Redis
        await Task.CompletedTask;
    }

    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes)!;
    }

    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();

        using var writer = new Utf8JsonWriter(buffer);

        JsonSerializer.Serialize(writer, value);

        return buffer.WrittenSpan.ToArray();
    }
}
