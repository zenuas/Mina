using System;
using System.Collections.Generic;

namespace Mina.Extension;

public static class Dictionaries
{
    public static TValue GetOrNew<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        where TKey : notnull
        where TValue : new()
    {
        if (!self.ContainsKey(key)) self.Add(key, new());
        return self[key];
    }

    public static TValue? GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        where TKey : notnull
        where TValue : notnull
    {
        return self.TryGetValue(key, out var value) ? value : default;
    }

    public static bool TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TValue> f)
        where TKey : notnull
    {
        return self.ContainsKey(key) ? false : self.TryAdd(key, f());
    }
}
