using System;
using System.Collections.Generic;

namespace Mina.Extension;

public static class Disposables
{
    public static void Using<T>(this T self, Action<T> f) where T : IDisposable
    {
        using var x = self;
        f(x);
    }

    public static R Using<T, R>(this T self, Func<T, R> f) where T : IDisposable
    {
        using var x = self;
        return f(x);
    }

    public static IEnumerable<R> UsingDefer<T, R>(this T self) where T : IDisposable, IEnumerable<R>
    {
        using var y = self;
        foreach (var x in y)
        {
            yield return x;
        }
    }

    public static IEnumerable<R> UsingDefer<T, R>(this T self, Func<T, IEnumerable<R>> f) where T : IDisposable
    {
        using var y = self;
        foreach (var x in f(y))
        {
            yield return x;
        }
    }
}
