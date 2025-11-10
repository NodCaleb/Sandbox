using System.Text;

namespace Sandbox.Leetcode;

public class _51_N_Queens
{
    private enum Cell
    {
        Free = 0,
        Queen = 1,
        Attacked = 2
    }

    [Theory]
    [InlineData(4, 2)]
    [InlineData(1, 1)]
    public void Test(int n, int expected)
    {
        var actual = SolveNQueens(n);
        Assert.Equal(expected, actual.Count);
    }

    private IList<IList<string>> SolveNQueens(int n)
    {
        var solutions = new List<Cell[,]>();

        var queue = new Queue<(Cell[,], int)>();
        queue.Enqueue((new Cell[n, n], 0));

        while (queue.Count > 0)
        {
            var board = queue.Dequeue();

            if (CalculateQueens(board.Item1, n) == n)
            {
                solutions.Add(board.Item1);
            }
            else
            {
                int cellCount = 0;

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        cellCount++;

                        if(cellCount > board.Item2 && board.Item1[i, j] == Cell.Free)
                        {
                            queue.Enqueue((PlaceQueen(board.Item1, i, j, n), cellCount));
                        }
                    }
                }
            }
        }

        return solutions.Select(s => PrintSolution(s, n)).ToList();
    }

    private Cell[,] PlaceQueen(Cell[,] oldBoard, int x, int y, int n)
    {
        var newBoard = new Cell[n, n];

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i == x && j == y || oldBoard[i, j] == Cell.Queen)
                {
                    newBoard[i, j] = Cell.Queen;
                }
                else if (
                    i == x ||
                    j == y ||
                    Math.Abs(i - x) == Math.Abs(j - y)
                    )
                {
                    newBoard[i, j] = Cell.Attacked;
                }
            }
        }

        return newBoard;
    }

    private int CalculateQueens(Cell[,] board, int n)
    {
        int queens = 0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] != Cell.Queen) queens++;
            }
        }

        return queens;
    }

    private IList<string> PrintSolution(Cell[,] board, int n)
    {
        var output = new List<string>();

        for (int i = 0; i < n; i++)
        {
            var sb = new StringBuilder();

            for (int j = 0; j < n; j++)
            {
                sb.Append(board[i, j] == Cell.Queen ? "Q" : ".");
            }

            output.Add(sb.ToString());
        }

        return output;
    }
}
