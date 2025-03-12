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
        int maxValue = nums.Length;
        bool onePresent = false;

        //Clean up the array
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == 1)
            {
                onePresent = true;
            }

            if (nums[i] > maxValue || nums[i] <= 0)
            {
                nums[i] = 1;
            }
        }

        if (!onePresent)
        {
            return 1;
        }

        //Mark the numbers
        for (int i = 0; i < nums.Length; i++)
        {
            nums[Math.Abs(nums[i]) - 1] = -Math.Abs(nums[Math.Abs(nums[i]) - 1]);
        }

        //Find the first missing positive
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] > 0)
            {
                return i + 1;
            }
        }

        return maxValue;
    }
}
