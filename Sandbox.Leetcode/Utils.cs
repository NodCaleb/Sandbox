using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Leetcode;

internal static class Utils
{
    public static int ComputeHash(IEnumerable<int> numbers)
    {
        int hash = 17;
        foreach (int num in numbers)
        {
            hash = HashCode.Combine(hash, num);
        }
        return hash;
    }
}
