using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interfaces.Services;

using Microsoft.Extensions.Caching.Memory;

namespace Application.Extensions;

public static class MemoryCacheExtensions
{
    public static T Set<T>(this IMemoryCacheExtended cache, object key, T value)
    {
        var entry = cache.CreateEntry(key);
        entry.Value = value;
        entry.Dispose();

        return value;
    }

    public static T Set<T>(this IMemoryCacheExtended cache, object key, T value, CacheItemPriority priority)
    {
        var entry = cache.CreateEntry(key);
        entry.Priority = priority;
        entry.Value = value;
        entry.Dispose();

        return value;
    }

    public static T Set<T>(this IMemoryCacheExtended cache, object key, T value, DateTimeOffset absoluteExpiration)
    {
        var entry = cache.CreateEntry(key);
        entry.AbsoluteExpiration = absoluteExpiration;
        entry.Value = value;
        entry.Dispose();

        return value;
    }

    public static T Set<T>(this IMemoryCacheExtended cache, object key, T value, TimeSpan absoluteExpirationRelativeToNow)
    {
        var entry = cache.CreateEntry(key);
        entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        entry.Value = value;
        entry.Dispose();

        return value;
    }

    public static T Set<T>(this IMemoryCacheExtended cache, object key, T value, MemoryCacheEntryOptions options)
    {
        using (var entry = cache.CreateEntry(key))
        {
            if (options != null)
                entry.SetOptions(options);

            entry.Value = value;
        }

        return value;
    }

    public static TItem GetOrCreate<TItem>(this IMemoryCacheExtended cache, object key, Func<ICacheEntry, TItem> factory)
    {
        if (!cache.TryGetValue(key, out var result))
        {
            var entry = cache.CreateEntry(key);
            result = factory(entry);
            entry.SetValue(result);
            entry.Dispose();
        }

        return (TItem)result;
    }

    public static async Task<TItem> GetOrCreateAsync<TItem>(this IMemoryCacheExtended cache, object key, Func<ICacheEntry, Task<TItem>> factory)
    {
        if (!cache.TryGetValue(key, out object result))
        {
            var entry = cache.CreateEntry(key);
            result = await factory(entry);
            entry.SetValue(result);
            entry.Dispose();
        }

        return (TItem)result;
    }
}