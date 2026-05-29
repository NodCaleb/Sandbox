namespace Sandbox.Leetcode;

public class _53_MaximumSubarray
{
    [Theory]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 6)]
    [InlineData(new int[] { 1 }, 1)]
    [InlineData(new int[] { 5, 4, -1, 7, 8 }, 23)]
    public void MaximumSubarray(int[] nums, int max)
    {
        var sum = MaxSubArray(nums);

        Assert.Equal(max, sum);
    }

    public int MaxSubArray(int[] nums)
    {
        int startIndex = 0;
        int endIndex = nums.Length - 1;

        int maxSum = 0;

        for (int i = 0; i < nums.Length; i++)
        {
            var sum = CalculateSubarraySum(nums, i, endIndex);
            if (sum > maxSum)
            {
                maxSum = sum;
                startIndex = i;
            }
        }

        for (int j = nums.Length - 1; j >= startIndex; j--)
        {
            var sum = CalculateSubarraySum(nums, startIndex, j);
            if (sum > maxSum)
            {
                maxSum = sum;
                endIndex = j;
            }
        }

        return maxSum;
    }

    private int CalculateSubarraySum(int[] nums, int startIndex, int endIndex)
    {
        int sum = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            sum += nums[i];
        }

        return sum;
    }
}
