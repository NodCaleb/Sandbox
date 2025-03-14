using System.Diagnostics;

namespace Sandbox.Leetcode;

public class _48_RotateImage
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[] {
            new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 }
            }
        };

        yield return new object[] {
            new int[][]
            {
                new int[] { 5, 1, 9, 11 },
                new int[] { 2, 4, 8, 10 },
                new int[] { 13, 3, 6, 7 },
                new int[] { 15, 14, 12, 16 }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void RotateTheMatrix(int[][] matrix)
    {
        PrintMatrix(matrix);
        Rotate(matrix);
        PrintMatrix(matrix);
    }

    private void Rotate(int[][] matrix)
    {
        int size = matrix.Length;
        int maxRowIndex = size / 2;
        int maxColumnIndex = size / 2 + size % 2;

        for (int i = 0; i < maxColumnIndex; i++)
        {
            for (int j = 0; j < maxRowIndex; j++)
            {
                int nextValue;
                int x = i;
                int y = j;
                int currentValue = matrix[x][y];

                for (int k = 0; k < 4; k++)
                {
                    var newIndices = GetRotatedIndices(x, y, size);
                    x = newIndices.Item1;
                    y = newIndices.Item2;
                    nextValue = matrix[x][y];
                    matrix[x][y] = currentValue;
                    currentValue = nextValue;
                }
            }
        }
    }

    //3: 0,0 > 0,2 > 2,2 > 2,0 > 0,0
    //3: 0,1 > 1,2 > 2,1 > 1,0 > 0,1
    //4: 0,1 > 1,3 > 3,2 > 2,0 > 0,1
    private (int, int) GetRotatedIndices(int x, int y, int matrixSize)
    {
        var maxIndex = matrixSize - 1;

        return (y, maxIndex - x);
    }

    private void PrintMatrix(int[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                Debug.Write($"{matrix[i][j]} ");
            }
            Debug.WriteLine("");
        }
        Debug.WriteLine("");
    }
}
