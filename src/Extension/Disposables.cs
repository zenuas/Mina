using System;

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
}
