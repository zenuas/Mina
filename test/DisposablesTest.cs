using Mina.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mina.Test;

public class DisposablesTest
{
    public class DisposeTest(string Name, List<string> Buffer) : IDisposable
    {
        public void Dispose()
        {
            Buffer.Add($"{Name} Dispose");
            GC.SuppressFinalize(this);
        }
    }

    [Fact]
    public void UsingTest()
    {
        var buffer = new List<string>();
        Assert.Equal(buffer.Join(", "), "");

        if (true)
        {
            using var a = new DisposeTest("a", buffer);
            using var b = new DisposeTest("b", buffer);
            Assert.Equal(buffer.Join(", "), "");
        }
        Assert.Equal(buffer.Join(", "), "b Dispose, a Dispose");

        new DisposeTest("x", buffer).Using(x => buffer.Add("dummy"));
        Assert.Equal(buffer.Join(", "), "b Dispose, a Dispose, dummy, x Dispose");

        var n = new DisposeTest("y", buffer).Using(y =>
        {
            buffer.Add("dummy2");
            Assert.Equal(buffer.Join(", "), "b Dispose, a Dispose, dummy, x Dispose, dummy2");
            return 123;
        });
        Assert.Equal(n, 123);
        Assert.Equal(buffer.Join(", "), "b Dispose, a Dispose, dummy, x Dispose, dummy2, y Dispose");
    }

    [Fact]
    public void UsingDeferTest()
    {
        var buffer = new List<string>();
        Assert.Equal(buffer.Join(", "), "");

        var xs = new DisposeTest("xs", buffer)
            .UsingDefer(_ => Lists.Sequence(0))
            .Take(3);

        var e = xs.GetEnumerator();
        Assert.Equal(buffer.Join(", "), "");
        Assert.Equal(e.MoveNext(), true);
        Assert.Equal(e.Current, 0);
        Assert.Equal(buffer.Join(", "), "");

        Assert.Equal(e.MoveNext(), true);
        Assert.Equal(e.Current, 1);
        Assert.Equal(buffer.Join(", "), "");

        Assert.Equal(e.MoveNext(), true);
        Assert.Equal(e.Current, 2);
        Assert.Equal(buffer.Join(", "), "");

        Assert.Equal(e.MoveNext(), false);
        Assert.Equal(buffer.Join(", "), "xs Dispose");

        var ys = new DisposeTest("ys", buffer)
            .UsingDefer(_ => new DisposeTest("zs", buffer).UsingDefer(_ => Lists.Sequence(0)))
            .Take(3);

        var e2 = ys.GetEnumerator();
        Assert.Equal(buffer.Join(", "), "xs Dispose");
        Assert.Equal(e2.MoveNext(), true);
        Assert.Equal(e2.Current, 0);
        Assert.Equal(buffer.Join(", "), "xs Dispose");

        Assert.Equal(e2.MoveNext(), true);
        Assert.Equal(e2.Current, 1);
        Assert.Equal(buffer.Join(", "), "xs Dispose");

        Assert.Equal(e2.MoveNext(), true);
        Assert.Equal(e2.Current, 2);
        Assert.Equal(buffer.Join(", "), "xs Dispose");

        Assert.Equal(e2.MoveNext(), false);
        Assert.Equal(buffer.Join(", "), "xs Dispose, zs Dispose, ys Dispose");
    }
}
