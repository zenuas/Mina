using Mina.Attributes;
using Mina.Extension;
using Xunit;

namespace Mina.Test;

public class EnumsTest
{
    public enum TestEnum
    {
        Aaa,

        [Alias("Xxx")]
        Bbb,

        Ccc,
    }

    [Fact]
    public void GetAttributeOrDefaultTest()
    {
        var a = TestEnum.Aaa.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(a, null);

        var b = TestEnum.Bbb.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(b?.Name, "Xxx");

        var c = TestEnum.Aaa.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(c, null);
    }

    [Fact]
    public void ParseTest()
    {
        var a = Enums.Parse<TestEnum>("Aaa");
        Assert.Equal(a, TestEnum.Aaa);

        var b = Enums.Parse<TestEnum>("Bbb");
        Assert.Equal(b, TestEnum.Bbb);

        var c = Enums.Parse<TestEnum>("Ccc");
        Assert.Equal(c, TestEnum.Ccc);

        var x = Enums.Parse<TestEnum>("Xxx");
        Assert.Equal(x, null);
    }

    [Fact]
    public void ParseWithAliasTest()
    {
        var a = Enums.ParseWithAlias<TestEnum>("Aaa");
        Assert.Equal(a, TestEnum.Aaa);

        var b = Enums.ParseWithAlias<TestEnum>("Bbb");
        Assert.Equal(b, TestEnum.Bbb);

        var c = Enums.ParseWithAlias<TestEnum>("Ccc");
        Assert.Equal(c, TestEnum.Ccc);

        var x = Enums.ParseWithAlias<TestEnum>("Xxx");
        Assert.Equal(x, TestEnum.Bbb);

        var y = Enums.ParseWithAlias<TestEnum>("Yyy");
        Assert.Equal(y, null);
    }
}
