using System.Diagnostics;

namespace Sandbox.Leetcode;

public class _44_WildcardMatching
{
    [Theory]
    [InlineData("abc", "abc", true)]
    [InlineData("aa", "a", false)]
    [InlineData("aa", "*", true)]
    [InlineData("cb", "?a", false)]
    [InlineData("adceb", "*a*b", true)]
    [InlineData("acdcb", "a*c?b", false)]
    public void WildcardMatch(string s, string p, bool expected)
    {
        Debug.WriteLine($"{s}, {p} => {expected}");
        Assert.Equal(expected, IsMatch(s, p));
    }

    private bool IsMatch(string s, string p)
    {
        int sIndex = 0;
        int pIndex = 0;

        while (sIndex < s.Length && pIndex < p.Length)
        {
            if (s[sIndex] == p[pIndex] || p[pIndex] == '?')
            {
                sIndex++;
                pIndex++;
            }
            else if (p[pIndex] == '*')
            {
                while (pIndex < p.Length && p[pIndex] == '*')
                {
                    pIndex++;
                }

                while (sIndex < s.Length && (pIndex == p.Length || s[sIndex] != p[pIndex]))
                {
                    sIndex++;
                }
            }
            else
            {
                return false;
            }                
        }

        return sIndex == s.Length && pIndex == p.Length;
    }
}
