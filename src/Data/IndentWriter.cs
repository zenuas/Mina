using System;
using System.IO;
using System.Text;

namespace Mina.Data;

public class IndentWriter : IDisposable
{
    public required Stream BaseStream { get; init; }
    public Encoding Encoding { get; init; } = Encoding.UTF8;
    public string IndentChars { get; init; } = "\t";
    public string LineBreakChars { get; init; } = "\n";
    public int Indent { get; set; } = 0;
    public bool IsNewLine { get; protected set; } = true;

    public void Write(char c)
    {
        if (IsNewLine && Indent > 0) WriteIndent(BaseStream, Indent, IndentChars, Encoding);
        IsNewLine = false;

        Span<byte> buffer = stackalloc byte[Encoding.GetMaxByteCount(1)];
        var count = Encoding.GetBytes([c], buffer);
        BaseStream.Write(buffer[..count]);
    }

    public void Write(string s)
    {
        if (IsNewLine && Indent > 0) WriteIndent(BaseStream, Indent, IndentChars, Encoding);
        IsNewLine = false;

        Span<byte> buffer = stackalloc byte[Encoding.GetMaxByteCount(s.Length)];
        var count = Encoding.GetBytes(s, buffer);
        BaseStream.Write(buffer[..count]);
    }

    public void WriteLine()
    {
        Write(LineBreakChars);
        IsNewLine = true;
    }

    public void WriteLine(string s)
    {
        Write(s);
        WriteLine();
    }

    public static void WriteIndent(Stream stream, int indent, string indent_chars, Encoding encoding)
    {
        Span<byte> buffer = stackalloc byte[encoding.GetMaxByteCount(indent_chars.Length)];
        var count = encoding.GetBytes(indent_chars, buffer);
        var chars = buffer[..count];
        for (var i = 0; i < indent; i++) stream.Write(chars);
    }

    public void Flush() => BaseStream.Flush();

    public void Close() => Dispose();

    public void Dispose()
    {
        BaseStream.Close();
        GC.SuppressFinalize(this);
    }
}
