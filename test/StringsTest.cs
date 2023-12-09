using Extensions;
using Xunit;

namespace Mina.Test;

public class StringsTest
{
    public StringsTest() => System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    [Fact]
    public void CountTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";
        var sjis = System.Text.Encoding.GetEncoding(932);

        Assert.Equal(s.CountAsByte(0, sjis), 0);
        Assert.Equal(s.CountAsByte(1, sjis), 1);
        Assert.Equal(s.CountAsByte(2, sjis), 2);
        Assert.Equal(s.CountAsByte(3, sjis), 3);
        Assert.Equal(s.CountAsByte(4, sjis), 3);
        Assert.Equal(s.CountAsByte(5, sjis), 4);
        Assert.Equal(s.CountAsByte(6, sjis), 4);
        Assert.Equal(s.CountAsByte(7, sjis), 5);
        Assert.Equal(s.CountAsByte(8, sjis), 5);
        Assert.Equal(s.CountAsByte(9, sjis), 6);
        Assert.Equal(s.CountAsByte(10, sjis), 6);
        Assert.Equal(s.CountAsByte(11, sjis), 7);
        Assert.Equal(s.CountAsByte(12, sjis), 7);
        Assert.Equal(s.CountAsByte(13, sjis), 8);
        Assert.Equal(s.CountAsByte(14, sjis), 8);
        Assert.Equal(s.CountAsByte(15, sjis), 9);
        Assert.Equal(s.CountAsByte(16, sjis), 9);
        Assert.Equal(s.CountAsByte(17, sjis), 10);
        Assert.Equal(s.CountAsByte(18, sjis), 10);
        Assert.Equal(s.CountAsByte(19, sjis), 11);
        Assert.Equal(s.CountAsByte(20, sjis), 11);
        Assert.Equal(s.CountAsByte(21, sjis), 12);
        Assert.Equal(s.CountAsByte(22, sjis), 13);
        Assert.Equal(s.CountAsByte(23, sjis), 14);
        Assert.Equal(s.CountAsByte(24, sjis), 15);
        Assert.Equal(s.CountAsByte(25, sjis), 15);
    }

    [Fact]
    public void SubstringTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";
        var sjis = System.Text.Encoding.GetEncoding(932);

        Assert.Equal(s.SubstringAsByte(0, 0, sjis), "");
        Assert.Equal(s.SubstringAsByte(0, 1, sjis), "a");
        Assert.Equal(s.SubstringAsByte(0, 2, sjis), "ab");
        Assert.Equal(s.SubstringAsByte(0, 3, sjis), "abc");
        Assert.Equal(s.SubstringAsByte(0, 4, sjis), "abc");
        Assert.Equal(s.SubstringAsByte(0, 5, sjis), "abcあ");
        Assert.Equal(s.SubstringAsByte(0, 6, sjis), "abcあ");
        Assert.Equal(s.SubstringAsByte(0, 7, sjis), "abcあい");
        Assert.Equal(s.SubstringAsByte(0, 8, sjis), "abcあい");
        Assert.Equal(s.SubstringAsByte(0, 9, sjis), "abcあいう");
        Assert.Equal(s.SubstringAsByte(0, 10, sjis), "abcあいう");
        Assert.Equal(s.SubstringAsByte(0, 11, sjis), "abcあいうア");
        Assert.Equal(s.SubstringAsByte(0, 12, sjis), "abcあいうア");
        Assert.Equal(s.SubstringAsByte(0, 13, sjis), "abcあいうアイ");
        Assert.Equal(s.SubstringAsByte(0, 14, sjis), "abcあいうアイ");
        Assert.Equal(s.SubstringAsByte(0, 15, sjis), "abcあいうアイウ");
        Assert.Equal(s.SubstringAsByte(0, 16, sjis), "abcあいうアイウ");
        Assert.Equal(s.SubstringAsByte(0, 17, sjis), "abcあいうアイウ亜");
        Assert.Equal(s.SubstringAsByte(0, 18, sjis), "abcあいうアイウ亜");
        Assert.Equal(s.SubstringAsByte(0, 19, sjis), "abcあいうアイウ亜伊");
        Assert.Equal(s.SubstringAsByte(0, 20, sjis), "abcあいうアイウ亜伊");
        Assert.Equal(s.SubstringAsByte(0, 21, sjis), "abcあいうアイウ亜伊宇");
        Assert.Equal(s.SubstringAsByte(0, 22, sjis), "abcあいうアイウ亜伊宇ｱ");
        Assert.Equal(s.SubstringAsByte(0, 23, sjis), "abcあいうアイウ亜伊宇ｱｲ");
        Assert.Equal(s.SubstringAsByte(0, 24, sjis), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(0, 25, sjis), "abcあいうアイウ亜伊宇ｱｲｳ");

        Assert.Equal(s.SubstringAsByte(0, 3, sjis), "abc");
        Assert.Equal(s.SubstringAsByte(1, 3, sjis), "bc");
        Assert.Equal(s.SubstringAsByte(2, 3, sjis), "cあ");
        Assert.Equal(s.SubstringAsByte(3, 3, sjis), "あ");
        Assert.Equal(s.SubstringAsByte(4, 3, sjis), "あ");
        Assert.Equal(s.SubstringAsByte(5, 3, sjis), "い");
        Assert.Equal(s.SubstringAsByte(6, 3, sjis), "い");
        Assert.Equal(s.SubstringAsByte(7, 3, sjis), "う");
        Assert.Equal(s.SubstringAsByte(8, 3, sjis), "う");
        Assert.Equal(s.SubstringAsByte(9, 3, sjis), "ア");
        Assert.Equal(s.SubstringAsByte(10, 3, sjis), "ア");
        Assert.Equal(s.SubstringAsByte(11, 3, sjis), "イ");
        Assert.Equal(s.SubstringAsByte(12, 3, sjis), "イ");
        Assert.Equal(s.SubstringAsByte(13, 3, sjis), "ウ");
        Assert.Equal(s.SubstringAsByte(14, 3, sjis), "ウ");
        Assert.Equal(s.SubstringAsByte(15, 3, sjis), "亜");
        Assert.Equal(s.SubstringAsByte(16, 3, sjis), "亜");
        Assert.Equal(s.SubstringAsByte(17, 3, sjis), "伊");
        Assert.Equal(s.SubstringAsByte(18, 3, sjis), "伊");
        Assert.Equal(s.SubstringAsByte(19, 3, sjis), "宇ｱ");
        Assert.Equal(s.SubstringAsByte(20, 3, sjis), "宇ｱ");
        Assert.Equal(s.SubstringAsByte(21, 3, sjis), "ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(22, 3, sjis), "ｲｳ");
        Assert.Equal(s.SubstringAsByte(23, 3, sjis), "ｳ");
        Assert.Equal(s.SubstringAsByte(24, 3, sjis), "");
    }
}
