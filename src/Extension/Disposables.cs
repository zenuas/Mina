using System;
using System.Collections;
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
        return new AutoDisposableAtLastEnumerable<R>(f(self), self.Dispose);
    }

    public class AutoDisposableAtLastEnumerable<T>(IEnumerable<T> BaseEnumerable, Action? DeferDispose = null) : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator() => new AutoDisposableAtLastEnumerator<T>(BaseEnumerable.GetEnumerator(), DeferDispose);

        IEnumerator IEnumerable.GetEnumerator() => new AutoDisposableAtLastEnumerator<T>(BaseEnumerable.GetEnumerator(), DeferDispose);
    }

    public class AutoDisposableAtLastEnumerator<T>(IEnumerator<T> BaseEnumerator, Action? DeferDispose = null) : IEnumerator<T>
    {
        public T Current => BaseEnumerator.Current;

        object IEnumerator.Current => BaseEnumerator.Current!;

        public void Dispose()
        {
            BaseEnumerator.Dispose();
            if (DeferDispose is { }) DeferDispose();
            GC.SuppressFinalize(this);
        }

        public bool MoveNext()
        {
            var b = BaseEnumerator.MoveNext();
            if (!b) Dispose();
            return b;
        }

        public void Reset() => BaseEnumerator.Reset();
    }
}
