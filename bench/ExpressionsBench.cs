using BenchmarkDotNet.Attributes;
using Mina.Reflections;
using System.Runtime.CompilerServices;

namespace Mina.Benchmark;

public class ExpressionsBench
{

    [MethodImpl(MethodImplOptions.NoInlining)]
    public int Dummy(int a, int b) => a + b;

    [Benchmark]
    public void Native()
    {
        Dummy(1, 2);
    }

    [Benchmark]
    public void LocalFunc()
    {
        void f() => Dummy(1, 2);
        f();
    }

    [Benchmark]
    public void LambdaFunc()
    {
        var f = () => Dummy(1, 2);
        f();
    }

    [Benchmark]
    public void MethodInvoke1()
    {
        var method = typeof(ExpressionsBench).GetMethod("Dummy")!;
        method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void MethodInvoke1000()
    {
        var method = typeof(ExpressionsBench).GetMethod("Dummy")!;
        for (var i = 0; i < 1000; i++) method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void MethodInvoke10000()
    {
        var method = typeof(ExpressionsBench).GetMethod("Dummy")!;
        for (var i = 0; i < 10000; i++) method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void ILGenerator1()
    {
        var f = Expressions.GetAction<ExpressionsBench, int, int>("Dummy");
        f(this, 1, 2);
    }

    [Benchmark]
    public void ILGenerator1000()
    {
        var f = Expressions.GetAction<ExpressionsBench, int, int>("Dummy");
        for (var i = 0; i < 1000; i++) f(this, 1, 2);
    }

    [Benchmark]
    public void ILGenerator10000()
    {
        var f = Expressions.GetAction<ExpressionsBench, int, int>("Dummy");
        for (var i = 0; i < 10000; i++) f(this, 1, 2);
    }
}
