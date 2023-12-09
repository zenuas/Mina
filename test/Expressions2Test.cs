using Extensions;
using Xunit;

namespace Mina.Test;

public class Expressions2Test
{
    public string Test { get; init; } = "xxx";
    public string Test2 { get; private set; } = "yyy";
    public int Test3 { get; private set; } = 0;

    public void Act1() => Test2 = "aaa";

    public void Act2(string s) => Test2 = s;

    public void Act3(int n) => Test3 = n;

    public string Func1() => Test2;

    public int Func2(int n) => n;

    [Fact]
    public void SetProperty()
    {
        var v = new Expressions2Test() { Test = "aaa" };
        var f = Expressions.SetProperty<Expressions2Test, string>("Test");
        f(v, "bbb");
        Assert.Equal(v.Test, "bbb");
    }

    [Fact]
    public void GetProperty()
    {
        var v = new Expressions2Test() { Test = "aaa" };
        var f = Expressions.GetProperty<Expressions2Test, string>("Test");
        var p = f(v);
        Assert.Equal(p, "aaa");
    }

    [Fact]
    public void GetAction1()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetAction<Expressions2Test>("Act1");
        f(v);
        Assert.Equal(v.Test2, "aaa");
    }

    [Fact]
    public void GetAction2()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetAction<Expressions2Test, string>("Act2");
        f(v, "bbb");
        Assert.Equal(v.Test2, "bbb");
    }

    [Fact]
    public void GetAction2_1()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetAction<Expressions2Test, int>("Act2");
        f(v, 123);
        Assert.Equal(v.Test2, "123");
    }

    [Fact]
    public void GetAction3()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetAction<Expressions2Test, long>("Act3");
        f(v, 123);
        Assert.Equal(v.Test3, 123);
    }

    [Fact]
    public void GetFunction1()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetFunction<Expressions2Test, string>("Func1");
        var p = f(v);
        Assert.Equal(p, "yyy");
    }

    [Fact]
    public void GetFunction2()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetFunction<Expressions2Test, int, int>("Func2");
        var p = f(v, 123);
        Assert.Equal(p, 123);
    }

    [Fact]
    public void GetFunction2_1()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetFunction<Expressions2Test, long, long>("Func2");
        var p = f(v, 123);
        Assert.Equal(p, 123);
    }

    [Fact]
    public void GetFunction2_2()
    {
        var v = new Expressions2Test();
        var f = Expressions.GetFunction<Expressions2Test, long, object>("Func2");
        var p = f(v, 123);
        Assert.True(p is int a && a == 123);
    }
}
