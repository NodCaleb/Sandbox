using System.Diagnostics;

namespace Sandbox.Leetcode;

public class _41_FirstMissingPositive
{
    [Theory]
    [InlineData(new int[] { 1, 2, 0 }, 3)]
    [InlineData(new int[] { 3, 4, -1, 1 }, 2)]
    [InlineData(new int[] { 7, 8, 9, 11, 12 }, 1)]
    public void FirstMissingPositive(int[] nums, int expected)
    {
        Debug.WriteLine($"[{string.Join(", ", nums)}] => {expected}");
        Assert.Equal(expected, GetFirstMissingPositive(nums));
    }

    int GetFirstMissingPositive(int[] nums)
    {
        int expected = 1;

        Array.Sort(nums);

        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == expected)
            {
                expected++;
            }
            if (nums[i] > expected)
            {
                return expected;
            }
        }

        return expected;
    }
}
