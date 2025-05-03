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

    public static IEnumerable<R> UsingDefer<T, R>(this T self, Func<T, IEnumerable<R>> f) where T : IDisposable
    {
        try
        {
            foreach (var x in f(self))
            {
                yield return x;
            }
        }
        finally
        {
            self.Dispose();
        }
    }
}
