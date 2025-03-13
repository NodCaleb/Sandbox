using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Leetcode;

public class _45_JumpGameII
{
    [Theory]
    [InlineData(new int[] { 2, 3, 1, 1, 4 }, 2)]
    [InlineData(new int[] { 2, 3, 0, 1, 4 }, 2)]
    public void JumpGame(int[] nums, int expected)
    {
        Console.WriteLine($"{string.Join(", ", nums)} => {expected}");
        Assert.Equal(expected, Jump(nums));
    }

    private int Jump(int[] nums)
    {
        var scores = new List<int>();
        var paths = new Queue<List<int>>();

        paths.Enqueue(new List<int>());

        while (paths.Count > 0)
        {
            var path = paths.Dequeue();

            var position = path.Sum();

            var maxJump = nums[position];

            for (int i = 1; i <= maxJump; i++)
            {
                if (position + i == nums.Length - 1)
                {
                    scores.Add(path.Count + 1);
                }
                else if (position + i < nums.Length - 1)
                {
                    var newPath = new List<int>();
                    newPath.AddRange(path);
                    newPath.Add(i);
                    paths.Enqueue(newPath);
                }
            }
        }

        return scores.Min();
    }
}
