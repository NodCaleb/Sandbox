namespace Sandbox.Leetcode;

public class _46_Permutations
{
    [Theory]
    [InlineData(new int[] { 1, 2, 3 }, 6)]
    [InlineData(new int[] { 0, 1 }, 2)]
    [InlineData(new int[] { 1 }, 1)]
    [InlineData(new int[] { 1, 2, 3, 4 }, 24)]
    [InlineData(new int[] { 10, 25, 300 }, 6)]
    public void Test(int[] nums, int expected)
    {
        var actual = Permute(nums);
        Assert.Equal(expected, actual.Count);
    }

    public IList<IList<int>> Permute(int[] nums)
    {
        var results = new List<IList<int>>();

        var queue = new Queue<List<int>>();

        queue.Enqueue(new List<int>());

        while (queue.Count > 0)
        {
            var entry = queue.Dequeue();

            if (entry.Count == nums.Length)
            {
                results.Add(entry);
            }
            else
            {
                for (int i = 0; i < nums.Length; i++)
                {
                    if (!entry.Contains(nums[i]))
                    {
                        var newEntry = new List<int>();
                        newEntry.AddRange(entry);
                        newEntry.Add(nums[i]);
                        queue.Enqueue(newEntry);
                    }
                }
            }
        }

        return results;
    }
}
