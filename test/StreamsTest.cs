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
}
