using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Extensions;
using System.Runtime.CompilerServices;

namespace Mina.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Program>();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Dummy() { }

    public void Native()
    {
        Dummy();
    }

    public void LocalFunc()
    {
        void f() => Dummy();
        f();
    }

    public void LambdaFunc()
    {
        var f = () => Dummy();
        f();
    }

    [Benchmark]
    public void MethodInvoke1()
    {
        var method = typeof(Program).GetMethod("Dummy")!;
        method.Invoke(this, []);
    }

    [Benchmark]
    public void MethodInvoke1000()
    {
        var method = typeof(Program).GetMethod("Dummy")!;
        for (var i = 0; i < 1000; i++) method.Invoke(this, []);
    }

    [Benchmark]
    public void ExpressionTree1()
    {
        var f = Expressions.GetAction<Program>("Dummy");
        f(this);
    }

    [Benchmark]
    public void ExpressionTree1000()
    {
        var f = Expressions.GetAction<Program>("Dummy");
        for (var i = 0; i < 1000; i++) f(this);
    }
}
