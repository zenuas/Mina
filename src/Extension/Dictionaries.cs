using System;
using System.Collections.Generic;

namespace Mina.Extension;

public static class Dictionaries
{
    public static TValue GetOrNew<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        where TKey : notnull
        where TValue : new()
        => self.GetOrNew(key, () => new());

    public static TValue GetOrNew<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TValue> f)
        where TKey : notnull
    {
        self.TryAdd(key, f);
        return self[key];
    }

    public static TValue? GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        where TKey : notnull
        where TValue : notnull
        => self.TryGetValue(key, out var value) ? value : default;

    public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TValue> f)
        where TKey : notnull
        => !self.ContainsKey(key) && self.TryAdd(key, f());
}
