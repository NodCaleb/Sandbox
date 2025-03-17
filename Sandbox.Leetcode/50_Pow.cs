namespace Sandbox.Leetcode;

public class _50_Pow
{
    [Theory]
    [InlineData(2.00000, 10, 1024.00000)]
    [InlineData(2.10000, 3, 9.26100)]
    [InlineData(2.00000, -2, 0.25000)]
    public void Test(double x, int n, double expected)
    {
        var actual = MyPow(x, n);
        Assert.Equal(expected, actual, 5);
    }

    private double MyPow(double x, int n)
    {
        if (x == 0 && n == 0)
            throw new ArgumentException();

        double result = 1;

        for (int i = 0; i < Math.Abs(n); i++)
        {
            result *= x;
        }

        if (n < 0)
        {
            result = 1 / result;
        }

        return result;
    }
}
