using BenchmarkDotNet.Attributes;
using Mina.Extensions;
using System.Text.RegularExpressions;

namespace Mina.Benchmark;

public class WildcardBench
{
    public Regex psc_ = null!;
    public string[] wil_ = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        psc_ = new Regex("^a.*0.*..*0.*f$", RegexOptions.Compiled);
        wil_ = Strings.WildcardAsteriskSplit("a*0*?*0*f");
    }

    [Benchmark]
    public void WildcardCompiled()
    {
        _ = Strings.MultiBlockMatch("abcdefabcdefabcdefabcdefabcdef01234567890abcdefabcdefabcdefabcdefabcdef", wil_);
    }

    [Benchmark]
    public void Wildcard()
    {
        _ = "abcdefabcdefabcdefabcdefabcdef01234567890abcdefabcdefabcdefabcdefabcdef".IsWildcardMatch("a*0*?*0*f");
    }

    [Benchmark]
    public void WildcardRegexCompiled()
    {
        _ = psc_.IsMatch("abcdefabcdefabcdefabcdefabcdef01234567890abcdefabcdefabcdefabcdefabcdef");
    }

    [Benchmark]
    public void WildcardRegex()
    {
        var ps = new Regex("^a.*0.*..*0.*f$");
        _ = ps.IsMatch("abcdefabcdefabcdefabcdefabcdef01234567890abcdefabcdefabcdefabcdefabcdef");
    }
}
