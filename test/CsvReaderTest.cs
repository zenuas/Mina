using Mina.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        Assert.Equal(["a"], CsvReader.TryReadFields(CreateReader("a"))!);
        Assert.Equal(["a", "b"], CsvReader.TryReadFields(CreateReader("a,b"))!);
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c"))!);
        Assert.Equal(["ab", "c"], CsvReader.TryReadFields(CreateReader("ab,c"))!);
        Assert.Equal(["a", "bc"], CsvReader.TryReadFields(CreateReader("a,bc"))!);
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\rx"))!);
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\nx"))!);
        Assert.Equal(["a", "b", "c"], CsvReader.TryReadFields(CreateReader("a,b,c\r\nx"))!);

        var input = CreateReader(@"name,age,addr
""foo,bar"",10,addr0
baz, 20,""addr1
addr2""

,
, ,  ,,
あいうえお,①,住所
");
        Assert.Equal(["name", "age", "addr"], CsvReader.TryReadFields(input)!);
        Assert.Equal(["foo,bar", "10", "addr0"], CsvReader.TryReadFields(input)!);
        Assert.Equal(["baz", " 20", "addr1\r\naddr2"], CsvReader.TryReadFields(input)!);
        Assert.Equal([""], CsvReader.TryReadFields(input)!);
        Assert.Equal(["", ""], CsvReader.TryReadFields(input)!);
        Assert.Equal(["", " ", "  ", "", ""], CsvReader.TryReadFields(input)!);
        Assert.Equal(["あいうえお", "①", "住所"], CsvReader.TryReadFields(input)!);
        Assert.Null(CsvReader.TryReadFields(input));
    }

    public class Data
    {
        public string Name { get; init; } = "";
        public int Age { get; init; } = -1;
        public string Address { get; init; } = "default";
    }

    [Fact]
    public void ReadMapperWithHeaderTest()
    {
        var nul = CsvReader.ReadMapperWithHeader<Data>(CreateReader("")).ToArray();
        Assert.Equal(nul.Length, 0);

        var input = CreateReader(@"Name,Age,Address
""foo,bar"",10,addr0
baz, 20,""addr1
addr2""
hoge,30,addr2,xxx
hogehoge,40
hogehogehoge

");
        var xs = CsvReader.ReadMapperWithHeader<Data>(input).ToArray();
        Assert.Equal(xs.Length, 6);
        Assert.Equivalent(xs[0], new Data { Name = "foo,bar", Age = 10, Address = "addr0" });
        Assert.Equivalent(xs[1], new Data { Name = "baz", Age = 20, Address = "addr1\r\naddr2" });
        Assert.Equivalent(xs[2], new Data { Name = "hoge", Age = 30, Address = "addr2" });
        Assert.Equivalent(xs[3], new Data { Name = "hogehoge", Age = 40, Address = "default" });
        Assert.Equivalent(xs[4], new Data { Name = "hogehogehoge", Age = -1, Address = "default" });
        Assert.Equivalent(xs[5], new Data { Name = "", Age = -1, Address = "default" });

        var input2 = CreateReader(@"Name,Age,Address,Dummy
foo,10,addr0,dummy
");
        var xs2 = CsvReader.ReadMapperWithHeader<Data>(input2).ToArray();
        Assert.Equal(xs2.Length, 1);
        Assert.Equivalent(xs2[0], new Data { Name = "foo", Age = 10, Address = "addr0" });

        var input3 = CreateReader(@"name,age,address,dummy
foo,10,addr0,dummy
");
        var xs3 = CsvReader.ReadMapperWithHeader<Data>(input3).ToArray();
        Assert.Equal(xs3.Length, 1);
        Assert.Equivalent(xs3[0], new Data { Name = "", Age = -1, Address = "default" });

        var input4 = CreateReader(@"Address,Age,Name,Dummy
foo,10,addr0,dummy
");
        var xs4 = CsvReader.ReadMapperWithHeader<Data>(input4).ToArray();
        Assert.Equal(xs4.Length, 1);
        Assert.Equivalent(xs4[0], new Data { Name = "addr0", Age = 10, Address = "foo" });

        var input5 = CreateReader(@"name,age,addr,dummy
foo,10,addr0,dummy
");
        Dictionary<string, string> header5 = new() {
            { "name", "Name" },
            { "age", "Age" },
            { "addr", "Address" },
            { "dummy", "Dummy" },
        };
        var xs5 = CsvReader.ReadMapperWithHeader<Data>(input5, s => header5[s]).ToArray();
        Assert.Equal(xs5.Length, 1);
        Assert.Equivalent(xs5[0], new Data { Name = "foo", Age = 10, Address = "addr0" });
    }

    [Fact]
    public void ReadMapperTest()
    {
        var input = CreateReader(@"foo,10,addr0
bar,20,addr1,dummy
baz,30

");
        string[] header = ["Name", "Age", "Address"];
        var xs = CsvReader.ReadMapper<Data>(input, i => i < header.Length ? header[i] : null).ToArray();
        Assert.Equal(xs.Length, 4);
        Assert.Equivalent(xs[0], new Data { Name = "foo", Age = 10, Address = "addr0" });
        Assert.Equivalent(xs[1], new Data { Name = "bar", Age = 20, Address = "addr1" });
        Assert.Equivalent(xs[2], new Data { Name = "baz", Age = 30, Address = "default" });
        Assert.Equivalent(xs[3], new Data { Name = "", Age = -1, Address = "default" });

        var input2 = CreateReader(@"foo,10,addr0
bar,20,addr1,dummy
baz,30

");
        var xs2 = CsvReader.ReadMapper<Data>(input2, i =>
            i == 0 ? header[2] :
            i == 2 ? header[0] :
            i < header.Length ? header[i]
            : null).ToArray();
        Assert.Equal(xs2.Length, 4);
        Assert.Equivalent(xs2[0], new Data { Name = "addr0", Age = 10, Address = "foo" });
        Assert.Equivalent(xs2[1], new Data { Name = "addr1", Age = 20, Address = "bar" });
        Assert.Equivalent(xs2[2], new Data { Name = "", Age = 30, Address = "baz" });
        Assert.Equivalent(xs2[3], new Data { Name = "", Age = -1, Address = "" });
    }
}
