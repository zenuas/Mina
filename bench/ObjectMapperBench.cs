using BenchmarkDotNet.Attributes;
using Mina.Mapper;

namespace Mina.Benchmark;

public class ObjectMapperBench
{
    record Data(int Int1, int Int2, string StringA, string StringB);

    [Benchmark]
    public void GetMapper1000()
    {
        var receiver = new Data(1, 2, "a", "b");
        var map_int = InstanceMapper.CreateGetMapper<Data, int>();
        var map_str = InstanceMapper.CreateGetMapper<Data, string>();
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
        var map_dynamic = InstanceMapper.CreateGetMapper<Data>();
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
        var map_int = InstanceMapper.CreateSetMapper<Data, int>();
        var map_str = InstanceMapper.CreateSetMapper<Data, string>();
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
        var map_dynamic = InstanceMapper.CreateSetMapper<Data>();
        for (var i = 0; i < 1000; i++)
        {
            map_dynamic["Int1"](receiver, 2);
            map_dynamic["Int2"](receiver, 4);
            map_dynamic["StringA"](receiver, "x");
            map_dynamic["StringB"](receiver, "y");
        }
    }
}
