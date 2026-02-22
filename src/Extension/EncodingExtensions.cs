using System.Text;

namespace Mina.Extension;

public static class EncodingExtensions
{
    extension(Encoding self)
    {
        public static Encoding? GetEncodingOrNull(int codepage) => Objects.TryOrDefault(() => Encoding.GetEncoding(codepage));
        public static Encoding? GetEncodingOrNull(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => Objects.TryOrDefault(() => Encoding.GetEncoding(codepage, encoderFallback, decoderFallback));
        public static Encoding? GetEncodingOrNull(string name) => Objects.TryOrDefault(() => Encoding.GetEncoding(name));
        public static Encoding? GetEncodingOrNull(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => Objects.TryOrDefault(() => Encoding.GetEncoding(name, encoderFallback, decoderFallback));

        public static Encoding GetEncodingOrUtf8(int codepage) => GetEncodingOrNull(codepage) ?? Encoding.UTF8;
        public static Encoding GetEncodingOrUtf8(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => GetEncodingOrNull(codepage, encoderFallback, decoderFallback) ?? Encoding.GetEncoding(65001, encoderFallback, decoderFallback);
        public static Encoding GetEncodingOrUtf8(string name) => GetEncodingOrNull(name) ?? Encoding.UTF8;
        public static Encoding GetEncodingOrUtf8(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback) => GetEncodingOrNull(name, encoderFallback, decoderFallback) ?? Encoding.GetEncoding(65001, encoderFallback, decoderFallback);

        public Encoding ToExceptionFallbackEncoding() => self.EncoderFallback == EncoderFallback.ExceptionFallback && self.DecoderFallback == DecoderFallback.ExceptionFallback ? self : Encoding.GetEncoding(self.CodePage, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

        public int? GetByteCountOrNull(string s) => Objects.TryOrDefault(() => self.ToExceptionFallbackEncoding().GetByteCount(s).To(x => x > 0 || s == "" ? (int?)x : null));
        public byte[]? GetBytesOrNull(string s) => Objects.TryOrDefault(() => self.ToExceptionFallbackEncoding().GetBytes(s));
        public int? GetCharCountOrNull(byte[] bytes) => Objects.TryOrDefault(() => self.ToExceptionFallbackEncoding().GetCharCount(bytes).To(x => x > 0 || bytes.Length == 0 ? (int?)x : null));
        public string? GetStringOrNull(byte[] bytes) => Objects.TryOrDefault(() => self.ToExceptionFallbackEncoding().GetString(bytes));
    }
}
