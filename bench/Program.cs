using BenchmarkDotNet.Attributes;
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

    record Data(int Int1, int Int2, string StringA, string StringB);

    [Benchmark]
    public void GetMapper1000()
    {
        var receiver = new Data(1, 2, "a", "b");
        var map_int = ObjectMapper.CreateGetMapper<Data, int>();
        var map_str = ObjectMapper.CreateGetMapper<Data, string>();
        for (var i = 0; i < 1000; i++)
        {
            var i1 = map_int["Int1"](receiver);
            var i2 = map_int["Int2"](receiver);
            var s1 = map_str["StringA"](receiver);
            var s2 = map_str["StringB"](receiver);

            if (!(i1 == 1)) throw new();
            if (!(i2 == 2)) throw new();
            if (!(s1 == "a")) throw new();
            if (!(s2 == "b")) throw new();
        }
    }

    [Benchmark]
    public void GetMapperD1000()
    {
        var receiver = new Data(1, 2, "a", "b");
        var map_dynamic = ObjectMapper.CreateGetMapper<Data>();
        for (var i = 0; i < 1000; i++)
        {
            var i1 = map_dynamic["Int1"](receiver);
            var i2 = map_dynamic["Int2"](receiver);
            var s1 = map_dynamic["StringA"](receiver);
            var s2 = map_dynamic["StringB"](receiver);

            if (!(i1 is int pi1 && pi1 == 1)) throw new();
            if (!(i2 is int pi2 && pi2 == 2)) throw new();
            if (!(s1 is string ps1 && ps1 == "a")) throw new();
            if (!(s2 is string ps2 && ps2 == "b")) throw new();
        }
    }

    [Benchmark]
    public void SetMapper1000()
    {
        var receiver = new Data(1, 2, "a", "b");
        var map_int = ObjectMapper.CreateSetMapper<Data, int>();
        var map_str = ObjectMapper.CreateSetMapper<Data, string>();
        for (var i = 0; i < 1000; i++)
        {
            map_int["Int1"](receiver, 2);
            map_int["Int2"](receiver, 4);
            map_str["StringA"](receiver, "x");
            map_str["StringB"](receiver, "y");
        }
    }

    [Benchmark]
    public void SetMapperD1000()
    {
        var receiver = new Data(1, 2, "a", "b");
        var map_dynamic = ObjectMapper.CreateSetMapper<Data>();
        for (var i = 0; i < 1000; i++)
        {
            map_dynamic["Int1"](receiver, 2);
            map_dynamic["Int2"](receiver, 4);
            map_dynamic["StringA"](receiver, "x");
            map_dynamic["StringB"](receiver, "y");
        }
    }
}
