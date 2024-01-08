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
    public static bool IsWildcardMatch(this string self, string pattern) => IsWildcardMatch(self, pattern, 0, 0);

    [DebuggerHidden]
    public static bool IsWildcardMatch(this string self, string pattern, int startIndex, int patternIndex)
    {
        if (self.Length <= startIndex && pattern.Length <= patternIndex) return true;
        if (self.Length <= startIndex && pattern.Length > patternIndex && pattern[patternIndex] == '*') return IsWildcardMatch(self, pattern, startIndex, patternIndex + 1);
        if (self.Length <= startIndex || pattern.Length <= patternIndex) return false;

        var c = pattern[patternIndex];
        return c switch
        {
            '*' => IsWildcardMatch(self, pattern, startIndex + 1, patternIndex) || IsWildcardMatch(self, pattern, startIndex + 1, patternIndex + 1),
            '?' => IsWildcardMatch(self, pattern, startIndex + 1, patternIndex + 1),
            _ => c == self[startIndex] && IsWildcardMatch(self, pattern, startIndex + 1, patternIndex + 1),
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
