using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Leetcode;

public class _52_N_Queens_II
{
    [Theory]
    [InlineData(4, 2)]
    [InlineData(1, 1)]
    public void Test(int n, int expected)
    {
        var actual = TotalNQueens(n);
        Assert.Equal(expected, actual);
    }

    private int TotalNQueens(int n)
    {
        var boards = new HashSet<QueenBoard>();

        for (int i = 0; i < n * n; i++)
        {
            var board = QueenBoard.ConstructBoard(n, i);

            if (board.CalculateQueens() == n && !boards.Contains(board)) boards.Add(board);
        }

        return boards.Count();
    }

    class QueenBoard
    {
        private int _size;
        private Cell[,] _cells { get; set; }

        public QueenBoard(int size)
        {
            _size = size;
            _cells = new Cell[size, size];
        }

        public int CalculateQueens()
        {
            int queens = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_cells[i, j] == Cell.Queen) queens++;
                }
            }

            return queens;
        }

        public IList<string> PrintBoard()
        {
            var output = new List<string>();

            for (int i = 0; i < _size; i++)
            {
                var sb = new StringBuilder();

                for (int j = 0; j < _size; j++)
                {
                    sb.Append(_cells[i, j] == Cell.Queen ? "Q" : ".");
                }

                output.Add(sb.ToString());
            }

            return output;
        }

        public void Fill(int offset = 0)
        {
            int cellsPassed = 0;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (cellsPassed >= offset)
                    {
                        if (_cells[i, j] == Cell.Free)
                        {
                            _cells[i, j] = Cell.Queen;
                            MarkAttackedCells(i, j);
                        }
                    }

                    cellsPassed++;
                }
            }
        }

        private void MarkAttackedCells(int x, int y)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (
                        !(i == x && j == y) && //Cannot attack itself
                        (
                            i == x ||
                            j == y ||
                            Math.Abs(i - x) == Math.Abs(j - y)
                        )
                    )
                    {
                        _cells[i, j] = Cell.Attacked;
                    }
                }
            }
        }

        public static QueenBoard ConstructBoard(int size, int offset)
        {
            if (offset >= size * size) throw new ArgumentException("Offset is not within the board");

            var board = new QueenBoard(size);
            board.Fill(offset);

            return board;
        }

    }
    private enum Cell
    {
        Free = 0,
        Queen = 1,
        Attacked = 2
    }
}
