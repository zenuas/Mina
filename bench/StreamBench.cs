using BenchmarkDotNet.Attributes;
using Mina.Extension;
using System.IO;
using System.Text;

namespace Mina.Benchmark;

public class StreamBench
{
    public MemoryStream mem_ = new();

    [GlobalSetup]
    public void GlobalSetup()
    {
        mem_ = new MemoryStream(Encoding.ASCII.GetBytes("abc123"));
    }

    [Benchmark]
    public void ReadSByte()
    {
        mem_.Position = 0;
        _ = mem_.ReadSByte();
    }

    [Benchmark]
    public void ReadUByte()
    {
        mem_.Position = 0;
        _ = mem_.ReadUByte();
    }
}
