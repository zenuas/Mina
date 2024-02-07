using BenchmarkDotNet.Attributes;
using System;
using System.Globalization;

namespace Mina.Benchmark;

public class SpeedBench
{
    [Benchmark]
    public void Bench1()
    {
        object o = "2000/01/01";
        DateTime n;

        n = (DateTime)Convert.ChangeType(o, typeof(DateTime), CultureInfo.InvariantCulture);
    }

    [Benchmark]
    public void Bench2()
    {
        object o = "2000/01/01";
        DateTime n;

        n = DateTime.Parse(o.ToString()!, CultureInfo.InvariantCulture);
    }

    [Benchmark]
    public void Bench3()
    {
        object o = "2000/01/01";
        DateTime n;

        DateTime.TryParse(o.ToString()!, CultureInfo.InvariantCulture, out n);
    }
}
