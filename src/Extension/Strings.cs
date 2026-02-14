using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mina.Extension;

public static class Strings
{
    public static int CountAsByte(this string self, int length, Encoding enc) => self
        .Select(x => enc.GetByteCount([x]))
        .Accumulator((acc, x) => acc + x)
        .TakeWhile(x => x <= length)
        .Count();

    public static string SubstringAsByte(this string self, int startIndex, Encoding enc) => self[self.CountAsByte(startIndex, enc)..];

    public static string SubstringAsByte(this string self, int startIndex, int length, Encoding enc) => self.SubstringAsByte(startIndex, enc).To(x => x[..x.CountAsByte(length, enc)]);

    public static string SubstringAsCount(this string self, int startIndex) => self[Math.Min(startIndex, self.Length)..];

    public static string SubstringAsCount(this string self, int startIndex, int length) => self[startIndex..Math.Min(startIndex + length, self.Length)];

    public static bool IsWildcardMatch(this string self, string pattern) => MultiBlockMatch(self, WildcardAsteriskSplit(pattern));

    public static string[] WildcardAsteriskSplit(string pattern)
    {
        var patterns = new List<string>();
        var start = 0;
        var length = 0;

        for (var i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == '*')
            {
                if (length > 0) patterns.Add(pattern[start..(start + length)]);
                for (i++; i < pattern.Length && pattern[i] == '*'; i++) ;
                start = i;
                length = 1;
            }
            else
            {
                length++;
            }
        }
        if (length > 0) patterns.Add(pattern[start..Math.Min(pattern.Length, start + length)]);
        return [.. patterns];
    }

    public static bool MultiBlockMatch(string s, string[] patterns)
    {
        if (patterns.Length == 0) return s.Length == 0;
        var span = s.AsSpan();
        var index = 0;
        for (var i = 0; i < patterns.Length - 1; i++)
        {
            var m = WildOrStringMatch(span[index..], patterns[i]);
            if (m < 0) return false;
            index += m + patterns[i].Length;
        }
        var last = patterns[^1];
        return span.Length >= last.Length && WildOrStringMatch(span[^last.Length..], last) >= 0;
    }

    public static int WildOrStringMatch(ReadOnlySpan<char> s, string pattern) =>
        pattern.Contains('?', StringComparison.Ordinal)
            ? Wild1Match(s, pattern)
            : s.IndexOf(pattern, StringComparison.Ordinal);

    public static int Wild1Match(ReadOnlySpan<char> s, string pattern)
    {
        if (pattern.Length == 0) return 0;
        for (var i = 0; i < s.Length - pattern.Length + 1; i++)
        {
            for (var pi = 0; pi < pattern.Length; pi++)
            {
                var pc = pattern[pi];
                if (pc != '?' && s[i + pi] != pc) break;
                if (pi == pattern.Length - 1) return i;
            }
        }
        return -1;
    }

    public static string Join(this IEnumerable<string> self, char separator) => string.Join(separator, self);

    public static string Join(this IEnumerable<string> self, string separator = "") => string.Join(separator, self);

    public static string[] SplitLine(this string self) => self.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);

    public static IEnumerable<int> ToUtf32CharArray(this string s)
    {
        var i = 0;
        for (; i < s.Length - 1; i++)
        {
            yield return char.IsHighSurrogate(s[i]) && char.IsLowSurrogate(s[i + 1])
                ? char.ConvertToUtf32(s[i], s[++i])
                : s[i];
        }
        if (i < s.Length) yield return s[^1];
    }

    public static string ToStringByChars(this IEnumerable<char> self) => new([.. self]);

    public static string ToStringByChars(this IEnumerable<int> chars) => chars.Select(char.ConvertFromUtf32).Join();
}
