using Mina.Extensions;
using System.IO;
using Xunit;

namespace Mina.Test;

public class StreamsTest
{
    [Fact]
    public void ReadLineTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal("", input.ReadLine());
        Assert.Equal("a", input.ReadLine());
        Assert.Equal("bc", input.ReadLine());
        Assert.Equal("def", input.ReadLine());
        Assert.Null(input.ReadLine());
        Assert.Null(input.ReadLine());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal("xyz", input2.ReadLine());
        Assert.Equal("a", input2.ReadLine());
        Assert.Equal("bc", input2.ReadLine());
        Assert.Equal("def", input2.ReadLine());
        Assert.Null(input2.ReadLine());
        Assert.Null(input2.ReadLine());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal("xyz", input3.ReadLine());
        Assert.Equal("a", input3.ReadLine());
        Assert.Equal("bc", input3.ReadLine());
        Assert.Equal("def", input3.ReadLine());
        Assert.Equal("", input3.ReadLine());
        Assert.Null(input3.ReadLine());
        Assert.Null(input3.ReadLine());
    }

    [Fact]
    public void ReadLineWithEolTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal("\n", input.ReadLineWithEol());
        Assert.Equal("a\r\n", input.ReadLineWithEol());
        Assert.Equal("bc\r", input.ReadLineWithEol());
        Assert.Equal("def", input.ReadLineWithEol());
        Assert.Equal("", input.ReadLineWithEol());
        Assert.Equal("", input.ReadLineWithEol());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal("xyz\n", input2.ReadLineWithEol());
        Assert.Equal("a\r\n", input2.ReadLineWithEol());
        Assert.Equal("bc\r", input2.ReadLineWithEol());
        Assert.Equal("def", input2.ReadLineWithEol());
        Assert.Equal("", input2.ReadLineWithEol());
        Assert.Equal("", input2.ReadLineWithEol());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal("xyz\n", input3.ReadLineWithEol());
        Assert.Equal("a\r\n", input3.ReadLineWithEol());
        Assert.Equal("bc\r", input3.ReadLineWithEol());
        Assert.Equal("def\n", input3.ReadLineWithEol());
        Assert.Equal("\n", input3.ReadLineWithEol());
        Assert.Equal("", input3.ReadLineWithEol());
        Assert.Equal("", input3.ReadLineWithEol());
    }

    [Fact]
    public void ReadLineSplitEolTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal(("", "\n"), input.ReadLineSplitEol());
        Assert.Equal(("a", "\r\n"), input.ReadLineSplitEol());
        Assert.Equal(("bc", "\r"), input.ReadLineSplitEol());
        Assert.Equal(("def", ""), input.ReadLineSplitEol());
        Assert.Equal(("", ""), input.ReadLineSplitEol());
        Assert.Equal(("", ""), input.ReadLineSplitEol());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal(("xyz", "\n"), input2.ReadLineSplitEol());
        Assert.Equal(("a", "\r\n"), input2.ReadLineSplitEol());
        Assert.Equal(("bc", "\r"), input2.ReadLineSplitEol());
        Assert.Equal(("def", ""), input2.ReadLineSplitEol());
        Assert.Equal(("", ""), input2.ReadLineSplitEol());
        Assert.Equal(("", ""), input2.ReadLineSplitEol());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal(("xyz", "\n"), input3.ReadLineSplitEol());
        Assert.Equal(("a", "\r\n"), input3.ReadLineSplitEol());
        Assert.Equal(("bc", "\r"), input3.ReadLineSplitEol());
        Assert.Equal(("def", "\n"), input3.ReadLineSplitEol());
        Assert.Equal(("", "\n"), input3.ReadLineSplitEol());
        Assert.Equal(("", ""), input3.ReadLineSplitEol());
        Assert.Equal(("", ""), input3.ReadLineSplitEol());
    }

    [Fact]
    public void ReadCharTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal('\n', input.PeekNewLineLF());
        Assert.Equal('\n', input.ReadNewLineLF());
        Assert.Equal('a', input.PeekNewLineLF());
        Assert.Equal('a', input.ReadNewLineLF());
        Assert.Equal('\n', input.PeekNewLineLF());
        Assert.Equal('\n', input.ReadNewLineLF());
        Assert.Equal('b', input.PeekNewLineLF());
        Assert.Equal('b', input.ReadNewLineLF());
        Assert.Equal('c', input.PeekNewLineLF());
        Assert.Equal('c', input.ReadNewLineLF());
        Assert.Equal('\n', input.PeekNewLineLF());
        Assert.Equal('\n', input.ReadNewLineLF());
        Assert.Equal('d', input.PeekNewLineLF());
        Assert.Equal('d', input.ReadNewLineLF());
        Assert.Equal('e', input.PeekNewLineLF());
        Assert.Equal('e', input.ReadNewLineLF());
        Assert.Equal('f', input.PeekNewLineLF());
        Assert.Equal('f', input.ReadNewLineLF());
        Assert.Null(input.PeekNewLineLF());
        Assert.Null(input.ReadNewLineLF());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal('x', input2.PeekNewLineLF());
        Assert.Equal('x', input2.ReadNewLineLF());
        Assert.Equal('y', input2.PeekNewLineLF());
        Assert.Equal('y', input2.ReadNewLineLF());
        Assert.Equal('z', input2.PeekNewLineLF());
        Assert.Equal('z', input2.ReadNewLineLF());
        Assert.Equal('\n', input2.PeekNewLineLF());
        Assert.Equal('\n', input2.ReadNewLineLF());
        Assert.Equal('a', input2.PeekNewLineLF());
        Assert.Equal('a', input2.ReadNewLineLF());
        Assert.Equal('\n', input2.PeekNewLineLF());
        Assert.Equal('\n', input2.ReadNewLineLF());
        Assert.Equal('b', input2.PeekNewLineLF());
        Assert.Equal('b', input2.ReadNewLineLF());
        Assert.Equal('c', input2.PeekNewLineLF());
        Assert.Equal('c', input2.ReadNewLineLF());
        Assert.Equal('\n', input2.PeekNewLineLF());
        Assert.Equal('\n', input2.ReadNewLineLF());
        Assert.Equal('d', input2.PeekNewLineLF());
        Assert.Equal('d', input2.ReadNewLineLF());
        Assert.Equal('e', input2.PeekNewLineLF());
        Assert.Equal('e', input2.ReadNewLineLF());
        Assert.Equal('f', input2.PeekNewLineLF());
        Assert.Equal('f', input2.ReadNewLineLF());
        Assert.Null(input2.PeekNewLineLF());
        Assert.Null(input2.ReadNewLineLF());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal('x', input3.PeekNewLineLF());
        Assert.Equal('x', input3.ReadNewLineLF());
        Assert.Equal('y', input3.PeekNewLineLF());
        Assert.Equal('y', input3.ReadNewLineLF());
        Assert.Equal('z', input3.PeekNewLineLF());
        Assert.Equal('z', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());
        Assert.Equal('\n', input3.ReadNewLineLF());
        Assert.Equal('a', input3.PeekNewLineLF());
        Assert.Equal('a', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());
        Assert.Equal('\n', input3.ReadNewLineLF());
        Assert.Equal('b', input3.PeekNewLineLF());
        Assert.Equal('b', input3.ReadNewLineLF());
        Assert.Equal('c', input3.PeekNewLineLF());
        Assert.Equal('c', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());
        Assert.Equal('\n', input3.ReadNewLineLF());
        Assert.Equal('d', input3.PeekNewLineLF());
        Assert.Equal('d', input3.ReadNewLineLF());
        Assert.Equal('e', input3.PeekNewLineLF());
        Assert.Equal('e', input3.ReadNewLineLF());
        Assert.Equal('f', input3.PeekNewLineLF());
        Assert.Equal('f', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());
        Assert.Equal('\n', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());
        Assert.Equal('\n', input3.ReadNewLineLF());
        Assert.Null(input3.PeekNewLineLF());
        Assert.Null(input3.ReadNewLineLF());
    }

    [Fact]
    public void ReadCharBadCaseTest()
    {
        var s = "a\r\nb";
        var input = new StringReader(s);
        Assert.Equal('a', input.PeekNewLineLF());
        Assert.Equal('a', input.ReadNewLineLF());
        Assert.Equal('\n', input.PeekNewLineLF());

        var c = input.Read();
        Assert.Equal('\r', c);

        Assert.Equal('\n', input.PeekNewLineLF());
        Assert.Equal('\n', input.ReadNewLineLF());
        Assert.Equal('b', input.PeekNewLineLF());
        Assert.Equal('b', input.ReadNewLineLF());
        Assert.Null(input.PeekNewLineLF());
        Assert.Null(input.ReadNewLineLF());

        var s2 = "a\rb";
        var input2 = new StringReader(s2);
        Assert.Equal('a', input2.PeekNewLineLF());
        Assert.Equal('a', input2.ReadNewLineLF());
        Assert.Equal('\n', input2.PeekNewLineLF());

        var c2 = input2.Read();
        Assert.Equal('\r', c2);

        Assert.Equal('b', input2.PeekNewLineLF());
        Assert.Equal('b', input2.ReadNewLineLF());
        Assert.Null(input2.PeekNewLineLF());
        Assert.Null(input2.ReadNewLineLF());

        var s3 = "a\nb";
        var input3 = new StringReader(s3);
        Assert.Equal('a', input3.PeekNewLineLF());
        Assert.Equal('a', input3.ReadNewLineLF());
        Assert.Equal('\n', input3.PeekNewLineLF());

        var c3 = input3.Read();
        Assert.Equal('\n', c3);

        Assert.Equal('b', input3.PeekNewLineLF());
        Assert.Equal('b', input3.ReadNewLineLF());
        Assert.Null(input3.PeekNewLineLF());
        Assert.Null(input3.ReadNewLineLF());
    }
}
