using Mina.Datas;
using System;
using System.IO;
using Xunit;

namespace Mina.Test;

public class CsvReaderTest
{
    public static TextReader CreateReader(string s) => new StringReader(s);

    [Fact]
    public void ReadRFC4180FieldTest()
    {
        Assert.Equal("", CsvReader.ReadRFC4180Field(CreateReader("")));
        Assert.Equal("a", CsvReader.ReadRFC4180Field(CreateReader("a")));
        Assert.Equal("a", CsvReader.ReadRFC4180Field(CreateReader("a,x")));
        Assert.Equal("ab", CsvReader.ReadRFC4180Field(CreateReader("ab,x")));
        Assert.Equal("a\"b", CsvReader.ReadRFC4180Field(CreateReader("a\"b,\"x")));
        Assert.Equal("a", CsvReader.ReadRFC4180Field(CreateReader("a\rx")));
        Assert.Equal("a", CsvReader.ReadRFC4180Field(CreateReader("a\nx")));
        Assert.Equal("a", CsvReader.ReadRFC4180Field(CreateReader("a\r\nx")));
    }

    [Fact]
    public void ReadRFC4180EscapedFieldTest()
    {
        _ = Assert.Throws<Exception>(() => CsvReader.ReadRFC4180EscapedField(CreateReader("")));

        Assert.Equal("a", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\"")));
        Assert.Equal("a", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\",x")));
        Assert.Equal("ab", CsvReader.ReadRFC4180EscapedField(CreateReader("\"ab\",x")));
        Assert.Equal("ab,", CsvReader.ReadRFC4180EscapedField(CreateReader("\"ab,\",x")));
        Assert.Equal("a\r", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\r\",x")));
        Assert.Equal("a\rb", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\rb\",x")));
        Assert.Equal("a\n", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\n\",x")));
        Assert.Equal("a\r\n", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\r\n\",x")));
        Assert.Equal("a\"b", CsvReader.ReadRFC4180EscapedField(CreateReader("\"a\"\"b\",x")));
    }

    [Fact]
    public void ReadFieldsTest()
    {
        _ = Assert.Throws<EndOfStreamException>(() => CsvReader.ReadFields(CreateReader("")));
        Assert.Equal(["a"], CsvReader.ReadFields(CreateReader("a")));
        Assert.Equal(["a", "b"], CsvReader.ReadFields(CreateReader("a,b")));
        Assert.Equal(["a", "b", "c"], CsvReader.ReadFields(CreateReader("a,b,c")));
        Assert.Equal(["ab", "c"], CsvReader.ReadFields(CreateReader("ab,c")));
        Assert.Equal(["a", "bc"], CsvReader.ReadFields(CreateReader("a,bc")));
        Assert.Equal(["a", "b", "c"], CsvReader.ReadFields(CreateReader("a,b,c\rx")));
        Assert.Equal(["a", "b", "c"], CsvReader.ReadFields(CreateReader("a,b,c\nx")));
        Assert.Equal(["a", "b", "c"], CsvReader.ReadFields(CreateReader("a,b,c\r\nx")));
    }

    [Fact]
    public void TryReadFieldsTest()
    {
        Assert.Null(CsvReader.TryReadFields(CreateReader("")));
        Assert.Equal(["a"], CsvReader.TryReadFields(CreateReader("a")));
        Assert.Equal(["a", "b"], CsvReader.TryReadFields(CreateReader("a,b")));
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c")));
        Assert.Equal(["ab", "c"], CsvReader.TryReadFields(CreateReader("ab,c")));
        Assert.Equal(["a", "bc"], CsvReader.TryReadFields(CreateReader("a,bc")));
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\rx")));
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\nx")));
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\r\nx")));

        var input = CreateReader(@"name,age,addr
""foo,bar"",10,addr0
baz, 20,""addr1
addr2""

,
, ,  ,,
あいうえお,①,住所
");
        Assert.Equal(["name", "age", "addr"], CsvReader.TryReadFields(input));
        Assert.Equal(["foo,bar", "10", "addr0"], CsvReader.TryReadFields(input));
        Assert.Equal(["baz", " 20", "addr1\r\naddr2"], CsvReader.TryReadFields(input));
        Assert.Equal([""], CsvReader.TryReadFields(input));
        Assert.Equal(["", ""], CsvReader.TryReadFields(input));
        Assert.Equal(["", " ", "  ", "", ""], CsvReader.TryReadFields(input));
        Assert.Equal(["あいうえお", "①", "住所"], CsvReader.TryReadFields(input));
        Assert.Null(CsvReader.TryReadFields(input));
    }
}
