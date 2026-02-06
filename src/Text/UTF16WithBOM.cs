using System.Text;

namespace Mina.Text;

public class UTF16WithBOM : UnicodeEncoding
{
    public static readonly UTF16WithBOM UTF16_BEWithBOM = new(true);
    public static readonly UTF16WithBOM UTF16_LEWithBOM = new(false);

    public UTF16WithBOM(bool bigEndian)
        : base(bigEndian, true)
    {
    }

    public UTF16WithBOM(bool bigEndian, bool throwOnInvalidByte)
        : base(bigEndian, true, throwOnInvalidByte)
    {
    }

    public override int GetByteCount(char[] chars, int index, int count) => base.GetByteCount(chars, index, count) + 2;

    public override int GetByteCount(string s) => base.GetByteCount(s) + 2;

    public override unsafe int GetByteCount(char* chars, int count) => chars is null ? 2 : base.GetByteCount(chars, count) + 2;

    public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        if (bytes.Length <= byteIndex) return 0;
        var bom = GetPreamble();
        bytes[byteIndex++] = bom[0];
        if (bytes.Length <= byteIndex) return 1;
        bytes[byteIndex++] = bom[1];

        return base.GetBytes(s, charIndex, charCount, bytes, byteIndex) + 2;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
        if (bytes.Length <= byteIndex) return 0;
        var bom = GetPreamble();
        bytes[byteIndex++] = bom[0];
        if (bytes.Length <= byteIndex) return 1;
        bytes[byteIndex++] = bom[1];

        return base.GetBytes(chars, charIndex, charCount, bytes, byteIndex) + 2;
    }

    public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
    {
        if (byteCount <= 0) return 0;
        var bom = GetPreamble();
        bytes[0] = bom[0];
        if (byteCount <= 1) return 1;
        bytes[1] = bom[1];

        return chars is null ? 2 : base.GetBytes(chars, charCount, bytes + 2, byteCount - 2) + 2;
    }
}
