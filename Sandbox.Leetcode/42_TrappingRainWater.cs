using System.Diagnostics;

namespace Sandbox.Leetcode;

public class _42_TrappingRainWater
{
    [Theory]
    [InlineData(new int[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 }, 6)]
    [InlineData(new int[] { 4, 2, 0, 3, 2, 5 }, 9)]
    public void Trap(int[] height, int expected)
    {
        Debug.WriteLine($"[{string.Join(", ", height)}] => {expected}");
        Assert.Equal(expected, GetTrappedRainWater(height));
    }

    public int GetTrappedRainWater(int[] height)
    {
        int highestIndex = 0;
        int highest = 0;

        for (int i = 0; i < height.Length; i++)
        {
            if (height[i] > highest)
            {
                highest = height[i];
                highestIndex = i;
            }
        }

        int trapped = 0;
        highest = 0;

        for (int i = 0; i < highestIndex; i++)
        {
            if (height[i] > highest)
            {
                highest = height[i];
            }
            else
            {
                trapped += highest - height[i];
            }
        }

        highest = 0;
        for (int i = height.Length - 1; i > highestIndex; i--)
        {
            if (height[i] > highest)
            {
                highest = height[i];
            }
            else
            {
                trapped += highest - height[i];
            }
        }

        return trapped;
    }
}
