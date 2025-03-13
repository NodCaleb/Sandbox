using System.Security;

namespace Sandbox.Leetcode;

public class _47_PermutationsII
{
    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, 6)]
    [InlineData(new int[] { 1, 1, 2 }, 3)]
    [InlineData(new int[] { 1, 1 }, 1)]
    [InlineData(new int[] { 1 }, 1)]
    [InlineData(new int[] { 1, 1, 1, 2 }, 4)]
    public void Test(int[] nums, int expected)
    {
        var actual = PermuteUnique(nums);
        Assert.Equal(expected, actual.Count);
    }

    public IList<IList<int>> PermuteUnique(int[] nums)
    {
        var sequences = new List<IList<int>>();

        var queue = new Queue<List<int>>();

        queue.Enqueue(new List<int>());

        while (queue.Count > 0)
        {
            var entry = queue.Dequeue();

            if (entry.Count == nums.Length)
            {
                sequences.Add(entry);
            }
            else
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    if (!entry.Contains(i))
                    {
                        var newEntry = new List<int>();
                        newEntry.AddRange(entry);
                        newEntry.Add(i);
                        queue.Enqueue(newEntry);
                    }
                }
            }
        }

        var results = new List<IList<int>>();

        foreach (var sequence in sequences)
        {
            var newResult = new List<int>();
            
            foreach (var index in sequence)
            {
                newResult.Add(nums[index]);
            }

            bool unique = true;

            foreach (var result in results)
            {
                if (!unique) break;

                var currentUnique = false;

                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i] != newResult[i]) currentUnique = true;
                }

                if (!currentUnique) unique = false;
            }

            if (unique)
            {
                results.Add(newResult);
            }
        }

        return results;
    }
}
