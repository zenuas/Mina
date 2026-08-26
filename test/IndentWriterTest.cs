using Mina.Data;
using System.IO;
using Xunit;

namespace Mina.Test;

public class IndentWriterTest
{
    [Fact]
    public void NullTest()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), []);
    }

    [Fact]
    public void WriteCharTest()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.Write('a');
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), [(byte)'a']);
    }

    [Fact]
    public void WriteCharWithIndentTest()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.Write('a');
        writer.WriteLine();
        writer.Indent += 1;
        writer.Write('b');
        writer.WriteLine();
        writer.Indent += 1;
        writer.Write('c');
        writer.WriteLine();
        writer.Indent -= 2;
        writer.Write('d');
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), [(byte)'a', (byte)'\n', (byte)'\t', (byte)'b', (byte)'\n', (byte)'\t', (byte)'\t', (byte)'c', (byte)'\n', (byte)'d']);
    }

    [Fact]
    public void WriteStringTest()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.Write("abc");
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), [(byte)'a', (byte)'b', (byte)'c']);
    }

    [Fact]
    public void WriteStringWithIndentTest()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.WriteLine("abc");
        writer.Indent += 1;
        writer.WriteLine("bcd");
        writer.Indent += 1;
        writer.WriteLine("cde");
        writer.Indent -= 2;
        writer.WriteLine("def");
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), [(byte)'a', (byte)'b', (byte)'c', (byte)'\n', (byte)'\t', (byte)'b', (byte)'c', (byte)'d', (byte)'\n', (byte)'\t', (byte)'\t', (byte)'c', (byte)'d', (byte)'e', (byte)'\n', (byte)'d', (byte)'e', (byte)'f', (byte)'\n']);
    }

    [Fact]
    public void WriteStringWithIndentTest2()
    {
        using var mem = new MemoryStream();
        using var writer = new IndentWriter() { BaseStream = mem };
        writer.Write("a");
        writer.WriteLine("bc");
        writer.Indent += 1;
        writer.Write("bc");
        writer.WriteLine("d");
        writer.Indent += 1;
        writer.Write("cde");
        writer.WriteLine();
        writer.Indent -= 2;
        writer.WriteLine("def");
        writer.Close();

        Assert.Equal<byte[]>(mem.ToArray(), [(byte)'a', (byte)'b', (byte)'c', (byte)'\n', (byte)'\t', (byte)'b', (byte)'c', (byte)'d', (byte)'\n', (byte)'\t', (byte)'\t', (byte)'c', (byte)'d', (byte)'e', (byte)'\n', (byte)'d', (byte)'e', (byte)'f', (byte)'\n']);
    }
}
