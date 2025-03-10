using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Leetcode;

public class _40_CombinationSumII
{
    [Theory]
    [InlineData(new int[] { 10, 1, 2, 7, 6, 1, 5 }, 8)]
    [InlineData(new int[] { 2, 5, 2, 1, 2 }, 5)]
    public void CombinationSum2(int[] candidates, int target)
    {
        Debug.WriteLine($"[{string.Join(", ", candidates)}] => {target}");
        Debug.WriteLine($"---");
        foreach (var combination in GetCombinations(candidates, target))
        {
            Debug.WriteLine($"[{string.Join(", ", combination)}]");
            Assert.True(combination.Sum() == target);
        }
    }


    IList<IList<int>> GetCombinations(int[] candidates, int target)
    {
        var combinations = new Dictionary<int, IList<int>>();

        var queue = new Queue<int[]>();

        queue.Enqueue(new int[candidates.Length]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var sum = 0;
            
            for (int i = 0; i < current.Length; i++)
            {
                sum += current[i] * candidates[i];
            }

            if (sum == target)
            {
                var combination = new List<int>();
                for (int i = 0; i < current.Length; i++)
                {
                    for (int j = 0; j < current[i]; j++)
                    {
                        combination.Add(candidates[i]);
                    }
                }

                combination.Sort();

                int key = Utils.ComputeHash(combination);

                if (!combinations.ContainsKey(key))
                    combinations.Add(key, combination);

            }
            else if (sum < target)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    var next = new int[current.Length];
                    Array.Copy(current, next, current.Length);

                    if (next[i] == 0)
                    {
                        next[i] = 1;
                        queue.Enqueue(next);
                    }                    
                }
            }
        }

        return combinations.Select(c => c.Value).ToList();
    }
}
