namespace Sandbox.Leetcode;

public class _39_CombinationSum
{
    [Theory]
    [InlineData(new int[] { 2, 3, 6, 7 }, 7)]
    [InlineData(new int[] { 2, 3, 5 }, 8)]
    [InlineData(new int[] { 2 }, 1)]
    public void CombinationSum(int[] candidates, int target)
    {
        Console.WriteLine($"[{string.Join(", ", candidates)}] => {target}");
        Console.WriteLine($"---");

        foreach (var combination in GetCombinations(candidates, target))
        {
            Console.WriteLine($"[{string.Join(", ", combination)}]");
            Assert.True(combination.Sum() == target);
        }
    }

    public IList<IList<int>> GetCombinations(int[] candidates, int target)
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

                int key = ComputeHash(combination);

                if (!combinations.ContainsKey(key))
                    combinations.Add(key, combination);
                
            }
            else if (sum < target)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    var next = new int[current.Length];
                    Array.Copy(current, next, current.Length);
                    next[i]++;
                    queue.Enqueue(next);
                }
            }
        }

        return combinations.Select(c => c.Value).ToList();
    }

    int ComputeHash(IEnumerable<int> numbers)
    {
        int hash = 17;
        foreach (int num in numbers)
        {
            hash = HashCode.Combine(hash, num);
        }
        return hash;
    }
}
