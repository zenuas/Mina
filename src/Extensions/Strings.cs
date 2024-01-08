using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Mina.Extensions;

public static class Strings
{
    [DebuggerHidden]
    public static int CountAsByte(this string self, int length, Encoding enc) =>
        self.Select(x => enc.GetByteCount([x]))
            .Accumulator((acc, x) => acc + x)
            .TakeWhile(x => x <= length)
            .Count();

    [DebuggerHidden]
    public static string SubstringAsByte(this string self, int startIndex, Encoding enc) => self[self.CountAsByte(startIndex, enc)..];

    [DebuggerHidden]
    public static string SubstringAsByte(this string self, int startIndex, int length, Encoding enc) => self.SubstringAsByte(startIndex, enc).To(x => x[..x.CountAsByte(length, enc)]);

    [DebuggerHidden]
    public static string SubstringAsCount(this string self, int startIndex) => self[Math.Min(startIndex, self.Length)..];

    [DebuggerHidden]
    public static string SubstringAsCount(this string self, int startIndex, int length) => self[startIndex..Math.Min(startIndex + length, self.Length)];

    [DebuggerHidden]
    public static bool IsWildcardMatch(this string self, string pattern) => IsWildcardMatch(self.AsSpan(), pattern.AsSpan());

    [DebuggerHidden]
    public static bool IsWildcardMatch(this ReadOnlySpan<char> self, ReadOnlySpan<char> pattern)
    {
        if (self.Length == 0 && pattern.Length == 0) return true;
        if (self.Length == 0 && pattern.Length >= 1 && pattern[0] == '*') return IsWildcardMatch(self, pattern[1..]);
        if (self.Length == 0 || pattern.Length == 0) return false;

        return pattern[0] switch
        {
            var c when c == '*' => IsWildcardMatch(self[1..], pattern) || IsWildcardMatch(self[1..], pattern[1..]),
            var c when c == '?' => IsWildcardMatch(self[1..], pattern[1..]),
            var c when c == self[0] => IsWildcardMatch(self[1..], pattern[1..]),
            _ => false,
        };
    }

    [DebuggerHidden]
    public static string Join(this IEnumerable<string> self, char separator) => string.Join(separator, self);

    [DebuggerHidden]
    public static string Join(this IEnumerable<string> self, string separator = "") => string.Join(separator, self);

    [DebuggerHidden]
    public static string[] SplitLine(this string self) => self.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);

    [DebuggerHidden]
    public static string ToStringByChars(this IEnumerable<char> self) => new([.. self]);
}
