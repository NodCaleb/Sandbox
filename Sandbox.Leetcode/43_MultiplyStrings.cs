using System.Diagnostics;

namespace Sandbox.Leetcode;

public class _43_MultiplyStrings
{
    [Theory]
    [InlineData("2", "3", "6")]
    [InlineData("123", "456", "56088")]
    public void Multiply(string num1, string num2, string expected)
    {
        Debug.WriteLine($"{num1} * {num2} => {expected}");
        Assert.Equal(expected, GetProduct(num1, num2));
    }

    string GetProduct(string num1, string num2)
    {
        var components = new List<string>();

        var resultDigits = new Stack<string>();
        var carry = 0;

        var digits0 = num1.Length > num2.Length ? num1.Reverse().ToArray() : num2.Reverse().ToArray();
        var digits1 = num1.Length > num2.Length ? num2.Reverse().ToArray() : num1.Reverse().ToArray();

        for (int i = 0; i < digits1.Length; i++)
        {
            for (int z = 0; z < i; z++)
            {
                resultDigits.Push("0");
            }

            var digit1 = CharToDigit(digits1[i]);

            for (int j = 0; j < digits0.Length; j++)
            {
                var digit0 = CharToDigit(digits0[j]);
                var product = digit0 * digit1 + carry;
                carry = product / 10;
                resultDigits.Push((product % 10).ToString());
            }

            if (carry > 0)
            {
                resultDigits.Push(carry.ToString());
                carry = 0;
            }

            components.Add(string.Join("", resultDigits));
            resultDigits.Clear();
        }

        var length = components.Select(c => c.Length).Max();

        for (int i = 0; i < length; i++)
        {
            var digits = components.Select(c => c.Length > i ? CharToDigit(c.Reverse().ElementAt(i)) : 0).ToArray();

            var sum = digits.Sum() + carry;
            carry = sum / 10;
            resultDigits.Push((sum % 10).ToString());
        }

        if (carry > 0)
        {
            resultDigits.Push(carry.ToString());
        }

        return string.Join("", resultDigits);
    }

    int CharToDigit(char input)
    {
        switch (input)
        {
            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            default:
                throw new ArgumentException();
        }
    }
}
