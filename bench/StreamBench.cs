using BenchmarkDotNet.Attributes;
using Mina.Extension;
using System.IO;
using System.Text;

namespace Mina.Benchmark;

public class StreamBench
{
    [Benchmark]
    public void WriteStringStackalloc()
    {
        var m = new MemoryStream();
        m.Write("abc123");
    }

    [Benchmark]
    public void WriteStringHeap()
    {
        var m = new MemoryStream();
        m.Write(Encoding.UTF8.GetBytes("abc123"));
    }
}
