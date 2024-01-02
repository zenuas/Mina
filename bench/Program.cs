﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Mina.Extensions;
using System.Runtime.CompilerServices;

namespace Mina.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        var config = new ManualConfig();
        config.AddLogger(NullLogger.Instance);
        config.AddExporter(new MarkdownConsoleExporter());
        config.AddColumnProvider(DefaultColumnProviders.Instance);
        config.WithOption(ConfigOptions.DisableOptimizationsValidator, true);
#else
        var config = DefaultConfig.Instance;
#endif

        BenchmarkRunner.Run<Program>(config, args);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public int Dummy(int a, int b) => a + b;

    public void Native()
    {
        Dummy(1, 2);
    }

    public void LocalFunc()
    {
        void f() => Dummy(1, 2);
        f();
    }

    public void LambdaFunc()
    {
        var f = () => Dummy(1, 2);
        f();
    }

    [Benchmark]
    public void MethodInvoke1()
    {
        var method = typeof(Program).GetMethod("Dummy")!;
        method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void MethodInvoke1000()
    {
        var method = typeof(Program).GetMethod("Dummy")!;
        for (var i = 0; i < 1000; i++) method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void MethodInvoke10000()
    {
        var method = typeof(Program).GetMethod("Dummy")!;
        for (var i = 0; i < 10000; i++) method.Invoke(this, [1, 2]);
    }

    [Benchmark]
    public void ILGenerator1()
    {
        var f = Expressions.GetAction<Program, int, int>("Dummy");
        f(this, 1, 2);
    }

    [Benchmark]
    public void ILGenerator1000()
    {
        var f = Expressions.GetAction<Program, int, int>("Dummy");
        for (var i = 0; i < 1000; i++) f(this, 1, 2);
    }

    [Benchmark]
    public void ILGenerator10000()
    {
        var f = Expressions.GetAction<Program, int, int>("Dummy");
        for (var i = 0; i < 10000; i++) f(this, 1, 2);
    }
}
