using Mina.Extension;
using System;
using Xunit;

namespace Mina.Test;

public class StringsTest
{
    public StringsTest() => System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    [Fact]
    public void CountAsByteTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";
        var sjis = System.Text.Encoding.GetEncoding(932);

        Assert.Equal(s.CountAsByte(-1, sjis), 0);
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
    public void SubstringAsByteTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";
        var sjis = System.Text.Encoding.GetEncoding(932);

        Assert.Equal(s.SubstringAsByte(-1, sjis), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(0, sjis), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(1, sjis), "bcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(2, sjis), "cあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(3, sjis), "あいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(4, sjis), "あいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(5, sjis), "いうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(6, sjis), "いうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(7, sjis), "うアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(8, sjis), "うアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(9, sjis), "アイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(10, sjis), "アイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(11, sjis), "イウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(12, sjis), "イウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(13, sjis), "ウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(14, sjis), "ウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(15, sjis), "亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(16, sjis), "亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(17, sjis), "伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(18, sjis), "伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(19, sjis), "宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(20, sjis), "宇ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(21, sjis), "ｱｲｳ");
        Assert.Equal(s.SubstringAsByte(22, sjis), "ｲｳ");
        Assert.Equal(s.SubstringAsByte(23, sjis), "ｳ");
        Assert.Equal(s.SubstringAsByte(24, sjis), "");
        Assert.Equal(s.SubstringAsByte(25, sjis), "");

        Assert.Equal(s.SubstringAsByte(0, -1, sjis), "");
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

        Assert.Equal(s.SubstringAsByte(-1, 3, sjis), "abc");
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

    [Fact]
    public void SubstringAsCountTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.SubstringAsCount(-1));
        Assert.Equal(s.SubstringAsCount(0), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(1), "bcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(2), "cあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(3), "あいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(4), "いうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(5), "うアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(6), "アイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(7), "イウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(8), "ウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(9), "亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(10), "伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(11), "宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(12), "ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(13), "ｲｳ");
        Assert.Equal(s.SubstringAsCount(14), "ｳ");
        Assert.Equal(s.SubstringAsCount(15), "");
        Assert.Equal(s.SubstringAsCount(16), "");

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.SubstringAsCount(0, -1));
        Assert.Equal(s.SubstringAsCount(0, 0), "");
        Assert.Equal(s.SubstringAsCount(0, 1), "a");
        Assert.Equal(s.SubstringAsCount(0, 2), "ab");
        Assert.Equal(s.SubstringAsCount(0, 3), "abc");
        Assert.Equal(s.SubstringAsCount(0, 4), "abcあ");
        Assert.Equal(s.SubstringAsCount(0, 5), "abcあい");
        Assert.Equal(s.SubstringAsCount(0, 6), "abcあいう");
        Assert.Equal(s.SubstringAsCount(0, 7), "abcあいうア");
        Assert.Equal(s.SubstringAsCount(0, 8), "abcあいうアイ");
        Assert.Equal(s.SubstringAsCount(0, 9), "abcあいうアイウ");
        Assert.Equal(s.SubstringAsCount(0, 10), "abcあいうアイウ亜");
        Assert.Equal(s.SubstringAsCount(0, 11), "abcあいうアイウ亜伊");
        Assert.Equal(s.SubstringAsCount(0, 12), "abcあいうアイウ亜伊宇");
        Assert.Equal(s.SubstringAsCount(0, 13), "abcあいうアイウ亜伊宇ｱ");
        Assert.Equal(s.SubstringAsCount(0, 14), "abcあいうアイウ亜伊宇ｱｲ");
        Assert.Equal(s.SubstringAsCount(0, 15), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(0, 16), "abcあいうアイウ亜伊宇ｱｲｳ");

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.SubstringAsCount(-1, 3));
        Assert.Equal(s.SubstringAsCount(0, 3), "abc");
        Assert.Equal(s.SubstringAsCount(1, 3), "bcあ");
        Assert.Equal(s.SubstringAsCount(2, 3), "cあい");
        Assert.Equal(s.SubstringAsCount(3, 3), "あいう");
        Assert.Equal(s.SubstringAsCount(4, 3), "いうア");
        Assert.Equal(s.SubstringAsCount(5, 3), "うアイ");
        Assert.Equal(s.SubstringAsCount(6, 3), "アイウ");
        Assert.Equal(s.SubstringAsCount(7, 3), "イウ亜");
        Assert.Equal(s.SubstringAsCount(8, 3), "ウ亜伊");
        Assert.Equal(s.SubstringAsCount(9, 3), "亜伊宇");
        Assert.Equal(s.SubstringAsCount(10, 3), "伊宇ｱ");
        Assert.Equal(s.SubstringAsCount(11, 3), "宇ｱｲ");
        Assert.Equal(s.SubstringAsCount(12, 3), "ｱｲｳ");
        Assert.Equal(s.SubstringAsCount(13, 3), "ｲｳ");
        Assert.Equal(s.SubstringAsCount(14, 3), "ｳ");
        Assert.Equal(s.SubstringAsCount(15, 3), "");
    }

    [Fact]
    public void SubstringTest()
    {
        var s = "abcあいうアイウ亜伊宇ｱｲｳ";

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(-1));
        Assert.Equal(s.Substring(0), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(1), "bcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(2), "cあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(3), "あいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(4), "いうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(5), "うアイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(6), "アイウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(7), "イウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(8), "ウ亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(9), "亜伊宇ｱｲｳ");
        Assert.Equal(s.Substring(10), "伊宇ｱｲｳ");
        Assert.Equal(s.Substring(11), "宇ｱｲｳ");
        Assert.Equal(s.Substring(12), "ｱｲｳ");
        Assert.Equal(s.Substring(13), "ｲｳ");
        Assert.Equal(s.Substring(14), "ｳ");
        Assert.Equal(s.Substring(15), "");
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(16));

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(0, -1));
        Assert.Equal(s.Substring(0, 0), "");
        Assert.Equal(s.Substring(0, 1), "a");
        Assert.Equal(s.Substring(0, 2), "ab");
        Assert.Equal(s.Substring(0, 3), "abc");
        Assert.Equal(s.Substring(0, 4), "abcあ");
        Assert.Equal(s.Substring(0, 5), "abcあい");
        Assert.Equal(s.Substring(0, 6), "abcあいう");
        Assert.Equal(s.Substring(0, 7), "abcあいうア");
        Assert.Equal(s.Substring(0, 8), "abcあいうアイ");
        Assert.Equal(s.Substring(0, 9), "abcあいうアイウ");
        Assert.Equal(s.Substring(0, 10), "abcあいうアイウ亜");
        Assert.Equal(s.Substring(0, 11), "abcあいうアイウ亜伊");
        Assert.Equal(s.Substring(0, 12), "abcあいうアイウ亜伊宇");
        Assert.Equal(s.Substring(0, 13), "abcあいうアイウ亜伊宇ｱ");
        Assert.Equal(s.Substring(0, 14), "abcあいうアイウ亜伊宇ｱｲ");
        Assert.Equal(s.Substring(0, 15), "abcあいうアイウ亜伊宇ｱｲｳ");
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(0, 16));

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(-1, 3));
        Assert.Equal(s.Substring(0, 3), "abc");
        Assert.Equal(s.Substring(1, 3), "bcあ");
        Assert.Equal(s.Substring(2, 3), "cあい");
        Assert.Equal(s.Substring(3, 3), "あいう");
        Assert.Equal(s.Substring(4, 3), "いうア");
        Assert.Equal(s.Substring(5, 3), "うアイ");
        Assert.Equal(s.Substring(6, 3), "アイウ");
        Assert.Equal(s.Substring(7, 3), "イウ亜");
        Assert.Equal(s.Substring(8, 3), "ウ亜伊");
        Assert.Equal(s.Substring(9, 3), "亜伊宇");
        Assert.Equal(s.Substring(10, 3), "伊宇ｱ");
        Assert.Equal(s.Substring(11, 3), "宇ｱｲ");
        Assert.Equal(s.Substring(12, 3), "ｱｲｳ");
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(13, 3));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(14, 3));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => s.Substring(15, 3));
    }

    [Fact]
    public void ToUtf32CharArrayTest()
    {
        Assert.Equal("".ToUtf32CharArray(), []);
        Assert.Equal("abcあいうアイウ亜伊宇ｱｲｳ".ToUtf32CharArray(), [0x61, 0x62, 0x63, 0x3042, 0x3044, 0x3046, 0x30a2, 0x30a4, 0x30a6, 0x4e9c, 0x4f0a, 0x5b87, 0xff71, 0xff72, 0xff73]);
        Assert.Equal("🍣".ToUtf32CharArray(), [0x1F363]);
        Assert.Equal("𠮷野家".ToUtf32CharArray(), [0x20BB7, 0x91CE, 0x5BB6]);
        Assert.Equal("abc\uD83C\uDF63".ToUtf32CharArray(), [0x61, 0x62, 0x63, 0x1F363]);
        Assert.Equal("\uD83C\uDF63abc".ToUtf32CharArray(), [0x1F363, 0x61, 0x62, 0x63]);
        Assert.Equal("abc\uD800".ToUtf32CharArray(), [0x61, 0x62, 0x63, 0xD800]); // unpaired surrogate
        Assert.Equal("\uD800a".ToUtf32CharArray(), [0xD800, 0x61]); // unpaired surrogate
    }

    [Fact]
    public void ToStringByCharsTest()
    {
        Assert.Equal(new int[] { }.ToStringByChars(), "");
        Assert.Equal(new int[] { 0x61, 0x62, 0x63, 0x3042, 0x3044, 0x3046, 0x30a2, 0x30a4, 0x30a6, 0x4e9c, 0x4f0a, 0x5b87, 0xff71, 0xff72, 0xff73 }.ToStringByChars(), "abcあいうアイウ亜伊宇ｱｲｳ");
        Assert.Equal(new int[] { 0x1F363 }.ToStringByChars(), "🍣");
        Assert.Equal(new int[] { 0x20BB7, 0x91CE, 0x5BB6 }.ToStringByChars(), "𠮷野家");
        Assert.Equal(new int[] { 0x61, 0x62, 0x63, 0x1F363 }.ToStringByChars(), "abc\uD83C\uDF63");
        Assert.Equal(new int[] { 0x1F363, 0x61, 0x62, 0x63 }.ToStringByChars(), "\uD83C\uDF63abc");
        Assert.Throws<ArgumentOutOfRangeException>(() => new int[] { 0x61, 0x62, 0x63, 0xD800 }.ToStringByChars()); // unpaired surrogate
        Assert.Throws<ArgumentOutOfRangeException>(() => new int[] { 0xD800, 0x61 }.ToStringByChars()); // unpaired surrogate
    }
}
