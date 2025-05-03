using Mina.Extension;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using Xunit;

namespace Mina.Test;

public class ListsTest
{
    public class TestEnumeratorAndDisposable(object[] lists, Action? disposable = null) : IEnumerator, IDisposable
    {
        public int Index { get; private set; } = 0;
        public object Current => lists[Index - 1];

        public bool MoveNext() => lists.Length >= ++Index;

        public void Reset() => Index = 0;

        public void Dispose() => disposable?.Invoke();
    }

    public class TestEnumerable(Func<IEnumerator> f) : IEnumerable
    {
        public IEnumerator GetEnumerator() => f();
    }

    [Fact]
    public void SequenceTest()
    {
        var xs = Lists.Sequence(1);
        var ys = Lists.RangeTo('a', 'f');
        var zs = xs.Zip(ys);
        var (a, b) = zs.UnZip();
        Assert.Equal(xs.Take(ys.Count()), a);
        Assert.Equal(ys, b);
    }

    [Fact]
    public void RangeTest()
    {
        var xs = Enumerable.Range(int.MaxValue - 5, 6);
        Assert.Equal(xs, new int[] {
                int.MaxValue - 5,
                int.MaxValue - 4,
                int.MaxValue - 3,
                int.MaxValue - 2,
                int.MaxValue - 1,
                int.MaxValue });

        var ys = Lists.Range((char)(char.MaxValue - 5), 6);
        Assert.Equal(ys, new char[] {
                (char)(char.MaxValue - 5),
                (char)(char.MaxValue - 4),
                (char)(char.MaxValue - 3),
                (char)(char.MaxValue - 2),
                (char)(char.MaxValue - 1),
                char.MaxValue });

        Assert.Equal(Enumerable.Range(0, 0), []);
        Assert.Equal(Lists.Range('A', 0), []);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(0, -1));
    }

    [Fact]
    public void RangeToTest()
    {
        var xs = Lists.RangeTo('a', 'z');
        var ys = Lists.RangeTo('A', 'Z');
        var zs = new (char, char)[] { ('a', 'z'), ('A', 'Z') }.Select(x => Lists.RangeTo(x.Item1, x.Item2)).Flatten();
        Assert.Equal(zs, xs.Concat(ys));
    }

    [Fact]
    public void NextTest()
    {
        var xs = new int[] { };
        var ys = new int[] { 1, 2, 3 };
        _ = Assert.Throws<IndexOutOfRangeException>(() => xs.Next());
        Assert.Equal(ys.Next(), [2, 3]);
    }

    [Fact]
    public void SortTest()
    {
        var xs = new (int, string)[] {
                (10, "a"),
                (11, "b"),
                (12, "c"),
                (11, "d"),
                (13, "e"),
                (10, "f"),
                (12, "g"),
                (14, "h"),
                (10, "i"),
                (11, "j"),
                (12, "k"),
                (11, "l"),
                (13, "m"),
                (10, "n"),
                (12, "o"),
                (14, "p"),
                (10, "q"),
            };

        var unstable_sort = xs.ToList();
        unstable_sort.Sort((x, y) => x.Item1 - y.Item1);
        var stable_sort = xs.Order((x, y) => x.Item1 - y.Item1).ToList();

        Assert.NotEqual(unstable_sort, stable_sort);

        Assert.Equal(stable_sort, new (int, string)[] {
                (10, "a"),
                (10, "f"),
                (10, "i"),
                (10, "n"),
                (10, "q"),
                (11, "b"),
                (11, "d"),
                (11, "j"),
                (11, "l"),
                (12, "c"),
                (12, "g"),
                (12, "k"),
                (12, "o"),
                (13, "e"),
                (13, "m"),
                (14, "h"),
                (14, "p"),
            });
    }

    [Fact]
    public void DropTest()
    {
        Assert.Equal("abc123".Skip(3).ToStringByChars(), "123");
        Assert.Equal("abc123"[0..^0].ToStringByChars(), "abc123");
        Assert.Equal("abc123"[0..^1].ToStringByChars(), "abc12");
        Assert.Equal("a"[0..^0].ToStringByChars(), "a");
        Assert.Equal("a"[0..^1].ToStringByChars(), "");
        Assert.Equal(""[0..^0].ToStringByChars(), "");
    }

    [Fact]
    public void TimeoutTest()
    {
        var xs = new int[] { 1, 2, 3, 1000, 5 };
        var r = xs.MapParallelAllWithTimeout(x =>
        {
            Thread.Sleep(x);
            return $"{x}_{x}";
        }, 100).ToArray();

        Assert.True(r[0].Completed);
        Assert.Equal(r[0].Result, "1_1");
        Assert.True(r[1].Completed);
        Assert.Equal(r[1].Result, "2_2");
        Assert.True(r[2].Completed);
        Assert.Equal(r[2].Result, "3_3");
        Assert.True(!r[3].Completed);
        Assert.Null(r[3].Result);
        Assert.True(r[4].Completed);
        Assert.Equal(r[4].Result, "5_5");
    }

    [Fact]
    public void SplitTest()
    {
        var xs1 = "".Split(',');
        Assert.Equal(xs1.Length, 1);

        var xs2 = "123,45,,6,".Split(',');
        Assert.Equal(xs2.Length, 5);
        Assert.Equal(xs2[0], "123");
        Assert.Equal(xs2[1], "45");
        Assert.Equal(xs2[2], "");
        Assert.Equal(xs2[3], "6");
        Assert.Equal(xs2[4], "");
    }

    [Fact]
    public void SplitInTest()
    {
        var xs1 = new int[] { }.SplitIn(x => x < 0).ToList();
        Assert.Equal(xs1.Count, 1);

        var xs2 = new int[] { 1, 2, 3, -1, 4, 5, -2, -3, 6, -4 }.SplitIn(x => x < 0).ToList();
        Assert.Equal(xs2.Count, 5);
        Assert.Equal(xs2[0], [1, 2, 3]);
        Assert.Equal(xs2[1], [4, 5]);
        Assert.Equal(xs2[2], []);
        Assert.Equal(xs2[3], [6]);
        Assert.Equal(xs2[4], []);
    }

    [Fact]
    public void SplitBeforeTest()
    {
        var xs1 = new int[] { }.SplitBefore(x => x < 0).ToList();
        Assert.Equal(xs1.Count, 1);

        var xs2 = new int[] { 1, 2, 3, -1, 4, 5, -2, -3, 6, -4 }.SplitBefore(x => x < 0).ToList();
        Assert.Equal(xs2.Count, 5);
        Assert.Equal(xs2[0], [1, 2, 3]);
        Assert.Equal(xs2[1], [-1, 4, 5]);
        Assert.Equal(xs2[2], [-2]);
        Assert.Equal(xs2[3], [-3, 6]);
        Assert.Equal(xs2[4], [-4]);
    }

    [Fact]
    public void SplitAfterTest()
    {
        var xs1 = new int[] { }.SplitAfter(x => x < 0).ToList();
        Assert.Equal(xs1.Count, 1);

        var xs2 = new int[] { 1, 2, 3, -1, 4, 5, -2, -3, 6, -4 }.SplitAfter(x => x < 0).ToList();
        Assert.Equal(xs2.Count, 5);
        Assert.Equal(xs2[0], [1, 2, 3, -1]);
        Assert.Equal(xs2[1], [4, 5, -2]);
        Assert.Equal(xs2[2], [-3]);
        Assert.Equal(xs2[3], [6, -4]);
        Assert.Equal(xs2[4], []);
    }

    [Fact]
    public void FirstLastTest()
    {
        var xs1 = new string[] { "one", "two", "three" };
        var xs2 = new string[] { };

        Assert.Equal(xs1.First(), "one");
        Assert.Equal(xs1.Last(), "three");
        Assert.Equal(xs1.FirstOrDefault(), "one");
        Assert.Equal(xs1.LastOrDefault(), "three");

        _ = Assert.Throws<InvalidOperationException>(() => xs2.First());
        _ = Assert.Throws<InvalidOperationException>(() => xs2.Last());
        Assert.Null(xs2.FirstOrDefault());
        Assert.Null(xs2.LastOrDefault());

        var xs3 = new int[] { 1, 2, 3 };
        var xs4 = new int[] { };

        Assert.Equal(xs3.First(), 1);
        Assert.Equal(xs3.Last(), 3);
        Assert.Equal(xs3.FindFirstOrNullValue(_ => true), 1);
        Assert.Equal(xs3.FindLastOrNullValue(_ => true), 3);

        _ = Assert.Throws<InvalidOperationException>(() => xs4.First());
        _ = Assert.Throws<InvalidOperationException>(() => xs4.Last());
        Assert.Null(xs4.FindFirstOrNullValue(_ => true));
        Assert.Null(xs4.FindLastOrNullValue(_ => true));
    }

    [Fact]
    public void GetIteratorTest()
    {
        var enumable = (IEnumerable)new string[] { "A", "B", "C" };

        var xs = enumable.GetIterator().OfType<string>().ToArray();
        Assert.IsType<string[]>(xs);
        Assert.Equal(xs.Length, 3);
        Assert.Equal(xs[0], "A");
        Assert.Equal(xs[1], "B");
        Assert.Equal(xs[2], "C");

        var dispose = 0;

        var e1 = new TestEnumerable(() => new TestEnumeratorAndDisposable([1, 2, 3, 4, 5]));
        Assert.Equal(e1.GetIterator().OfType<int>().ToList(), [1, 2, 3, 4, 5]);
        Assert.Equal(dispose, 0);

        var e2 = new TestEnumerable(() => new TestEnumeratorAndDisposable([1, 2, 3, 4, 5]));
        Assert.Equal(e2.GetIterator().OfType<int>().Take(3), [1, 2, 3]);
        Assert.Equal(dispose, 0);

        var e3 = new TestEnumerable(() => new TestEnumeratorAndDisposable([1, 2, 3, 4, 5], () => dispose++));
        Assert.Equal(e3.GetIterator().OfType<int>().ToList(), [1, 2, 3, 4, 5]);
        Assert.Equal(dispose, 1);
        Assert.Equal(e3.GetIterator().OfType<int>().ToList(), [1, 2, 3, 4, 5]);
        Assert.Equal(dispose, 2);

        var e4 = new TestEnumerable(() => new TestEnumeratorAndDisposable([1, 2, 3, 4, 5], () => dispose++));
        Assert.Equal(e4.GetIterator().OfType<int>().Take(3), [1, 2, 3]);
        Assert.Equal(dispose, 3);

        var xs4_1 = e4.GetIterator().OfType<int>();
        Assert.Equal(dispose, 3);
        foreach (var x in xs4_1)
        {
            if (x == 3) break;
        }
        Assert.Equal(dispose, 4);

        var xs4_2 = e4.GetIterator().OfType<int>();
        Assert.Equal(dispose, 4);
        try
        {
            foreach (var x in xs4_2)
            {
                if (x == 3) throw new Exception();
            }
        }
        catch
        {
            Assert.Equal(dispose, 5);
        }
    }
}
