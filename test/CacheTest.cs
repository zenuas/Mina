using Extensions;
using Xunit;
using System;

namespace UnderBridge.Test;

public class CacheTest
{
    [Fact]
    public void MemoTest()
    {
        var count = 0;
        int fib(int x)
        {
            count++;
            return x <= 1 ? 1 : fib(x - 1) + fib(x - 2);
        }

        count = 0;
        Assert.Equal(fib(0), 1);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fib(1), 1);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fib(2), 2);
        Assert.Equal(count, 3);

        count = 0;
        Assert.Equal(fib(3), 3);
        Assert.Equal(count, 5);

        count = 0;
        Assert.Equal(fib(4), 5);
        Assert.Equal(count, 9);

        count = 0;
        Assert.Equal(fib(10), 89);
        Assert.Equal(count, 177);

        Func<int, int> fibmemo = x => 1;
        int fib2(int x)
        {
            count++;
            return x <= 1 ? 1 : fibmemo(x - 1) + fibmemo(x - 2);
        }
        fibmemo = Cache.Memoization<int, int>(fib2);

        count = 0;
        Assert.Equal(fibmemo(0), 1);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fibmemo(1), 1);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fibmemo(2), 2);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fibmemo(3), 3);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fibmemo(4), 5);
        Assert.Equal(count, 1);

        count = 0;
        Assert.Equal(fibmemo(10), 89);
        Assert.Equal(count, 6);
    }
}
