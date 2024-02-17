using Mina.Extension;
using System.IO;
using System.Linq;
using Xunit;

namespace Mina.Test;

public class StreamsTest
{
    [Fact]
    public void ReadLineTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal(input.ReadLine(), "");
        Assert.Equal(input.ReadLine(), "a");
        Assert.Equal(input.ReadLine(), "bc");
        Assert.Equal(input.ReadLine(), "def");
        Assert.Null(input.ReadLine());
        Assert.Null(input.ReadLine());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal(input2.ReadLine(), "xyz");
        Assert.Equal(input2.ReadLine(), "a");
        Assert.Equal(input2.ReadLine(), "bc");
        Assert.Equal(input2.ReadLine(), "def");
        Assert.Null(input2.ReadLine());
        Assert.Null(input2.ReadLine());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal(input3.ReadLine(), "xyz");
        Assert.Equal(input3.ReadLine(), "a");
        Assert.Equal(input3.ReadLine(), "bc");
        Assert.Equal(input3.ReadLine(), "def");
        Assert.Equal(input3.ReadLine(), "");
        Assert.Null(input3.ReadLine());
        Assert.Null(input3.ReadLine());
    }

    [Fact]
    public void ReadAllLinesTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s).ReadAllLines().ToArray();
        Assert.Equal(input.Length, 4);
        Assert.Equal(input[0], "");
        Assert.Equal(input[1], "a");
        Assert.Equal(input[2], "bc");
        Assert.Equal(input[3], "def");

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2).ReadAllLines().ToArray();
        Assert.Equal(input2.Length, 4);
        Assert.Equal(input2[0], "xyz");
        Assert.Equal(input2[1], "a");
        Assert.Equal(input2[2], "bc");
        Assert.Equal(input2[3], "def");

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3).ReadAllLines().ToArray();
        Assert.Equal(input3.Length, 5);
        Assert.Equal(input3[0], "xyz");
        Assert.Equal(input3[1], "a");
        Assert.Equal(input3[2], "bc");
        Assert.Equal(input3[3], "def");
        Assert.Equal(input3[4], "");
    }

    [Fact]
    public void ReadLineWithEolTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal(input.ReadLineWithEol(), "\n");
        Assert.Equal(input.ReadLineWithEol(), "a\r\n");
        Assert.Equal(input.ReadLineWithEol(), "bc\r");
        Assert.Equal(input.ReadLineWithEol(), "def");
        Assert.Equal(input.ReadLineWithEol(), "");
        Assert.Equal(input.ReadLineWithEol(), "");

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal(input2.ReadLineWithEol(), "xyz\n");
        Assert.Equal(input2.ReadLineWithEol(), "a\r\n");
        Assert.Equal(input2.ReadLineWithEol(), "bc\r");
        Assert.Equal(input2.ReadLineWithEol(), "def");
        Assert.Equal(input2.ReadLineWithEol(), "");
        Assert.Equal(input2.ReadLineWithEol(), "");

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal(input3.ReadLineWithEol(), "xyz\n");
        Assert.Equal(input3.ReadLineWithEol(), "a\r\n");
        Assert.Equal(input3.ReadLineWithEol(), "bc\r");
        Assert.Equal(input3.ReadLineWithEol(), "def\n");
        Assert.Equal(input3.ReadLineWithEol(), "\n");
        Assert.Equal(input3.ReadLineWithEol(), "");
        Assert.Equal(input3.ReadLineWithEol(), "");
    }

    [Fact]
    public void ReadAllLineWithEolsTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s).ReadAllLineWithEols().ToArray();
        Assert.Equal(input.Length, 4);
        Assert.Equal(input[0], "\n");
        Assert.Equal(input[1], "a\r\n");
        Assert.Equal(input[2], "bc\r");
        Assert.Equal(input[3], "def");

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2).ReadAllLineWithEols().ToArray();
        Assert.Equal(input2.Length, 4);
        Assert.Equal(input2[0], "xyz\n");
        Assert.Equal(input2[1], "a\r\n");
        Assert.Equal(input2[2], "bc\r");
        Assert.Equal(input2[3], "def");

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3).ReadAllLineWithEols().ToArray();
        Assert.Equal(input3.Length, 5);
        Assert.Equal(input3[0], "xyz\n");
        Assert.Equal(input3[1], "a\r\n");
        Assert.Equal(input3[2], "bc\r");
        Assert.Equal(input3[3], "def\n");
        Assert.Equal(input3[4], "\n");
    }

    [Fact]
    public void ReadLineSplitEolTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal(input.ReadLineSplitEol(), ("", "\n"));
        Assert.Equal(input.ReadLineSplitEol(), ("a", "\r\n"));
        Assert.Equal(input.ReadLineSplitEol(), ("bc", "\r"));
        Assert.Equal(input.ReadLineSplitEol(), ("def", ""));
        Assert.Equal(input.ReadLineSplitEol(), ("", ""));
        Assert.Equal(input.ReadLineSplitEol(), ("", ""));

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal(input2.ReadLineSplitEol(), ("xyz", "\n"));
        Assert.Equal(input2.ReadLineSplitEol(), ("a", "\r\n"));
        Assert.Equal(input2.ReadLineSplitEol(), ("bc", "\r"));
        Assert.Equal(input2.ReadLineSplitEol(), ("def", ""));
        Assert.Equal(input2.ReadLineSplitEol(), ("", ""));
        Assert.Equal(input2.ReadLineSplitEol(), ("", ""));

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal(input3.ReadLineSplitEol(), ("xyz", "\n"));
        Assert.Equal(input3.ReadLineSplitEol(), ("a", "\r\n"));
        Assert.Equal(input3.ReadLineSplitEol(), ("bc", "\r"));
        Assert.Equal(input3.ReadLineSplitEol(), ("def", "\n"));
        Assert.Equal(input3.ReadLineSplitEol(), ("", "\n"));
        Assert.Equal(input3.ReadLineSplitEol(), ("", ""));
        Assert.Equal(input3.ReadLineSplitEol(), ("", ""));
    }

    [Fact]
    public void ReadAllLineSplitEolsTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s).ReadAllLineSplitEols().ToArray();
        Assert.Equal(input.Length, 4);
        Assert.Equal(input[0], ("", "\n"));
        Assert.Equal(input[1], ("a", "\r\n"));
        Assert.Equal(input[2], ("bc", "\r"));
        Assert.Equal(input[3], ("def", ""));

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2).ReadAllLineSplitEols().ToArray();
        Assert.Equal(input2.Length, 4);
        Assert.Equal(input2[0], ("xyz", "\n"));
        Assert.Equal(input2[1], ("a", "\r\n"));
        Assert.Equal(input2[2], ("bc", "\r"));
        Assert.Equal(input2[3], ("def", ""));

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3).ReadAllLineSplitEols().ToArray();
        Assert.Equal(input3.Length, 5);
        Assert.Equal(input3[0], ("xyz", "\n"));
        Assert.Equal(input3[1], ("a", "\r\n"));
        Assert.Equal(input3[2], ("bc", "\r"));
        Assert.Equal(input3[3], ("def", "\n"));
        Assert.Equal(input3[4], ("", "\n"));
    }

    [Fact]
    public void ReadCharTest()
    {
        var s = "\na\r\nbc\rdef";
        var input = new StringReader(s);
        Assert.Equal(input.PeekNewLineLF(), '\n');
        Assert.Equal(input.ReadNewLineLF(), '\n');
        Assert.Equal(input.PeekNewLineLF(), 'a');
        Assert.Equal(input.ReadNewLineLF(), 'a');
        Assert.Equal(input.PeekNewLineLF(), '\n');
        Assert.Equal(input.ReadNewLineLF(), '\n');
        Assert.Equal(input.PeekNewLineLF(), 'b');
        Assert.Equal(input.ReadNewLineLF(), 'b');
        Assert.Equal(input.PeekNewLineLF(), 'c');
        Assert.Equal(input.ReadNewLineLF(), 'c');
        Assert.Equal(input.PeekNewLineLF(), '\n');
        Assert.Equal(input.ReadNewLineLF(), '\n');
        Assert.Equal(input.PeekNewLineLF(), 'd');
        Assert.Equal(input.ReadNewLineLF(), 'd');
        Assert.Equal(input.PeekNewLineLF(), 'e');
        Assert.Equal(input.ReadNewLineLF(), 'e');
        Assert.Equal(input.PeekNewLineLF(), 'f');
        Assert.Equal(input.ReadNewLineLF(), 'f');
        Assert.Null(input.PeekNewLineLF());
        Assert.Null(input.ReadNewLineLF());

        var s2 = "xyz\na\r\nbc\rdef";
        var input2 = new StringReader(s2);
        Assert.Equal(input2.PeekNewLineLF(), 'x');
        Assert.Equal(input2.ReadNewLineLF(), 'x');
        Assert.Equal(input2.PeekNewLineLF(), 'y');
        Assert.Equal(input2.ReadNewLineLF(), 'y');
        Assert.Equal(input2.PeekNewLineLF(), 'z');
        Assert.Equal(input2.ReadNewLineLF(), 'z');
        Assert.Equal(input2.PeekNewLineLF(), '\n');
        Assert.Equal(input2.ReadNewLineLF(), '\n');
        Assert.Equal(input2.PeekNewLineLF(), 'a');
        Assert.Equal(input2.ReadNewLineLF(), 'a');
        Assert.Equal(input2.PeekNewLineLF(), '\n');
        Assert.Equal(input2.ReadNewLineLF(), '\n');
        Assert.Equal(input2.PeekNewLineLF(), 'b');
        Assert.Equal(input2.ReadNewLineLF(), 'b');
        Assert.Equal(input2.PeekNewLineLF(), 'c');
        Assert.Equal(input2.ReadNewLineLF(), 'c');
        Assert.Equal(input2.PeekNewLineLF(), '\n');
        Assert.Equal(input2.ReadNewLineLF(), '\n');
        Assert.Equal(input2.PeekNewLineLF(), 'd');
        Assert.Equal(input2.ReadNewLineLF(), 'd');
        Assert.Equal(input2.PeekNewLineLF(), 'e');
        Assert.Equal(input2.ReadNewLineLF(), 'e');
        Assert.Equal(input2.PeekNewLineLF(), 'f');
        Assert.Equal(input2.ReadNewLineLF(), 'f');
        Assert.Null(input2.PeekNewLineLF());
        Assert.Null(input2.ReadNewLineLF());

        var s3 = "xyz\na\r\nbc\rdef\n\n";
        var input3 = new StringReader(s3);
        Assert.Equal(input3.PeekNewLineLF(), 'x');
        Assert.Equal(input3.ReadNewLineLF(), 'x');
        Assert.Equal(input3.PeekNewLineLF(), 'y');
        Assert.Equal(input3.ReadNewLineLF(), 'y');
        Assert.Equal(input3.PeekNewLineLF(), 'z');
        Assert.Equal(input3.ReadNewLineLF(), 'z');
        Assert.Equal(input3.PeekNewLineLF(), '\n');
        Assert.Equal(input3.ReadNewLineLF(), '\n');
        Assert.Equal(input3.PeekNewLineLF(), 'a');
        Assert.Equal(input3.ReadNewLineLF(), 'a');
        Assert.Equal(input3.PeekNewLineLF(), '\n');
        Assert.Equal(input3.ReadNewLineLF(), '\n');
        Assert.Equal(input3.PeekNewLineLF(), 'b');
        Assert.Equal(input3.ReadNewLineLF(), 'b');
        Assert.Equal(input3.PeekNewLineLF(), 'c');
        Assert.Equal(input3.ReadNewLineLF(), 'c');
        Assert.Equal(input3.PeekNewLineLF(), '\n');
        Assert.Equal(input3.ReadNewLineLF(), '\n');
        Assert.Equal(input3.PeekNewLineLF(), 'd');
        Assert.Equal(input3.ReadNewLineLF(), 'd');
        Assert.Equal(input3.PeekNewLineLF(), 'e');
        Assert.Equal(input3.ReadNewLineLF(), 'e');
        Assert.Equal(input3.PeekNewLineLF(), 'f');
        Assert.Equal(input3.ReadNewLineLF(), 'f');
        Assert.Equal(input3.PeekNewLineLF(), '\n');
        Assert.Equal(input3.ReadNewLineLF(), '\n');
        Assert.Equal(input3.PeekNewLineLF(), '\n');
        Assert.Equal(input3.ReadNewLineLF(), '\n');
        Assert.Null(input3.PeekNewLineLF());
        Assert.Null(input3.ReadNewLineLF());
    }

    [Fact]
    public void ReadCharBadCaseTest()
    {
        var s = "a\r\nb";
        var input = new StringReader(s);
        Assert.Equal(input.PeekNewLineLF(), 'a');
        Assert.Equal(input.ReadNewLineLF(), 'a');
        Assert.Equal(input.PeekNewLineLF(), '\n');

        var c = input.Read();
        Assert.Equal('\r', c);

        Assert.Equal(input.PeekNewLineLF(), '\n');
        Assert.Equal(input.ReadNewLineLF(), '\n');
        Assert.Equal(input.PeekNewLineLF(), 'b');
        Assert.Equal(input.ReadNewLineLF(), 'b');
        Assert.Null(input.PeekNewLineLF());
        Assert.Null(input.ReadNewLineLF());

        var s2 = "a\rb";
        var input2 = new StringReader(s2);
        Assert.Equal(input2.PeekNewLineLF(), 'a');
        Assert.Equal(input2.ReadNewLineLF(), 'a');
        Assert.Equal(input2.PeekNewLineLF(), '\n');

        var c2 = input2.Read();
        Assert.Equal('\r', c2);

        Assert.Equal(input2.PeekNewLineLF(), 'b');
        Assert.Equal(input2.ReadNewLineLF(), 'b');
        Assert.Null(input2.PeekNewLineLF());
        Assert.Null(input2.ReadNewLineLF());

        var s3 = "a\nb";
        var input3 = new StringReader(s3);
        Assert.Equal(input3.PeekNewLineLF(), 'a');
        Assert.Equal(input3.ReadNewLineLF(), 'a');
        Assert.Equal(input3.PeekNewLineLF(), '\n');

        var c3 = input3.Read();
        Assert.Equal('\n', c3);

        Assert.Equal(input3.PeekNewLineLF(), 'b');
        Assert.Equal(input3.ReadNewLineLF(), 'b');
        Assert.Null(input3.PeekNewLineLF());
        Assert.Null(input3.ReadNewLineLF());
    }
}
