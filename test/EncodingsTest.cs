using Mina.Extension;
using System;
using System.Text;
using Xunit;

namespace Mina.Test;

public class EncodingsTest
{
    [Fact]
    public void GetEncodingTest()
    {
        Assert.Throws<ArgumentException>(() => Encoding.GetEncoding(""));
        Assert.Throws<ArgumentException>(() => Encoding.GetEncoding("xxx_not_found_xxx"));
        Assert.Throws<NotSupportedException>(() => Encoding.GetEncoding(65535));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoding.GetEncoding(-1));
        Assert.Equal(Encoding.GetEncoding(1200), Encoding.Unicode);
    }

    [Fact]
    public void GetEncodingOrNullTest()
    {
        Assert.Equal(Encodings.GetEncodingOrNull(""), null);
        Assert.Equal(Encodings.GetEncodingOrNull("xxx_not_found_xxx"), null);
        Assert.Equal(Encodings.GetEncodingOrNull(65535), null);
        Assert.Equal(Encodings.GetEncodingOrNull(-1), null);
        Assert.Equal(Encodings.GetEncodingOrNull(1200), Encoding.Unicode);
    }

    [Fact]
    public void GetEncodingOrUtf8Test()
    {
        var utf8 = Encoding.UTF8;

        Assert.Equal(Encodings.GetEncodingOrUtf8(""), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8("xxx_not_found_xxx"), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(65535), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(-1), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(1200), Encoding.Unicode);
    }
}
