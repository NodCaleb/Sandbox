using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Leetcode;

public class _38_SudokuSolver
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[]
        {
            new char[][]
            {
                new char[] { '5', '3', '.', '.', '7', '.', '.', '.', '.' },
                new char[] { '6', '.', '.', '1', '9', '5', '.', '.', '.' },
                new char[] { '.', '9', '8', '.', '.', '.', '.', '6', '.' },
                new char[] { '8', '.', '.', '.', '6', '.', '.', '.', '3' },
                new char[] { '4', '.', '.', '8', '.', '3', '.', '.', '1' },
                new char[] { '7', '.', '.', '.', '2', '.', '.', '.', '6' },
                new char[] { '.', '6', '.', '.', '.', '.', '2', '8', '.' },
                new char[] { '.', '.', '.', '4', '1', '9', '.', '.', '5' },
                new char[] { '.', '.', '.', '.', '8', '.', '.', '7', '9' }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void SolveSudoku(char[][] board)
    {
        var bv = SolveBoard(board);
        Assert.NotNull(bv);
    }

    BoardVolatile SolveBoard(char[][] board)
    {
        int solutionLength = 0;
        var queue = new Queue<List<char>>();

        var bv = new BoardVolatile(board);
        bv.CheckBoard();

        //Find rows, columns and sets, where any of the values is possible only on one place, place it there and repeat
        int optionsSet;

        do
        {
            optionsSet = 0;

            var possiblePositions = new Dictionary<char, List<(int, int)>>();
            //Rows
            for (int i = 0; i < 9; i++)
            {
                possiblePositions.Clear();

                for (int j = 0; j < 9; j++)
                {
                    var opions = bv.GetOptions(i, j);
                    if (opions.Count == 1) continue;

                    foreach (var option in opions)
                    {
                        if (possiblePositions.ContainsKey(option))
                        {
                            possiblePositions[option].Add((i, j));
                        }
                        else
                        {
                            possiblePositions[option] = new List<(int, int)> { (i, j) };
                        }
                    }
                }

                foreach (var item in possiblePositions.Where(p => p.Value.Count == 1))
                {
                    bv.SetOption(item.Value.First().Item1, item.Value.First().Item2, item.Key);
                    optionsSet++;
                }
                bv.CheckBoard();
            }

            //Columns
            for (int j = 0; j < 9; j++)
            {
                possiblePositions.Clear();

                for (int i = 0; i < 9; i++)
                {
                    var opions = bv.GetOptions(i, j);
                    if (opions.Count == 1) continue;

                    foreach (var option in opions)
                    {
                        if (possiblePositions.ContainsKey(option))
                        {
                            possiblePositions[option].Add((i, j));
                        }
                        else
                        {
                            possiblePositions[option] = new List<(int, int)> { (i, j) };
                        }
                    }
                }

                foreach (var item in possiblePositions.Where(p => p.Value.Count == 1))
                {
                    bv.SetOption(item.Value.First().Item1, item.Value.First().Item2, item.Key);
                    optionsSet++;
                }
                bv.CheckBoard();
            }

            //Sets
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    possiblePositions.Clear();

                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            var opions = bv.GetOptions(i * 3 + k, j * 3 + l);
                            if (opions.Count == 1) continue;

                            foreach (var option in opions)
                            {
                                if (possiblePositions.ContainsKey(option))
                                {
                                    possiblePositions[option].Add((i * 3 + k, j * 3 + l));
                                }
                                else
                                {
                                    possiblePositions[option] = new List<(int, int)> { (i * 3 + k, j * 3 + l) };
                                }
                            }
                        }
                    }

                    foreach (var item in possiblePositions.Where(p => p.Value.Count == 1))
                    {
                        bv.SetOption(item.Value.First().Item1, item.Value.First().Item2, item.Key);
                        optionsSet++;
                    }
                    bv.CheckBoard();
                }
            }
        } while (optionsSet > 0);

        var volatility = bv.GetVolatility();

        if (volatility == 0) return null;

        if (volatility == 1) return bv;

        //Solving by traversing options tree
        foreach (var o in bv.GetFirstOptions()) queue.Enqueue(new List<char> { o });
        solutionLength = 1;

        while (queue.Count > 0)
        {
            var solution = queue.Dequeue();

            if (solution.Count > solutionLength)
            {
                solutionLength = solution.Count;
                Console.WriteLine($"Solution length: {solutionLength}");
            }

            var iBv = new BoardVolatile(board);
            iBv.SetOptions(solution);
            iBv.CalculateOptions();
            var iVolatility = iBv.GetVolatility();

            if (volatility == 0) continue;
            else if (volatility == 1) return bv;
            else
            {
                var options = iBv.GetFirstOptions();

                foreach (var o in options)
                {
                    var newSolution = new List<char>();
                    newSolution.AddRange(solution);
                    newSolution.Add(o);
                    queue.Enqueue(newSolution);
                }
            }
        }

        return null;
    }
}

class BoardVolatile
{
    List<char>[,] _options;

    char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    public BoardVolatile(char[][] board)
    {
        _options = new List<char>[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _options[i, j] = new List<char>();
                if (board[i][j] != '.') _options[i, j].Add(board[i][j]);
            }
        }
    }

    public int GetVolatility()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count == 0) return 0;
                if (_options[i, j].Count > 1) return 2;
            }
        }
        return 1;
    }

    public List<char> GetFirstOptions()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count > 1) return _options[i, j];
            }
        }
        return new List<char>();
    }

    public void SetFirstOption(char value)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count == 0)
                {
                    _options[i, j] = new List<char> { value };
                    return;
                }
            }
        }
    }

    public void SetOptions(List<char> values)
    {
        foreach (char c in values)
        {
            SetFirstOption(c);
        }
    }

    public void FillSolution(char[][] board)
    {
        if (GetVolatility() != 1) throw new Exception("Solution not found");

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                board[i][j] = _options[i, j].First();
            }
        }
    }

    public void CalculateCellOptions(int x, int y)
    {
        var values = new List<char>();

        //Row
        for (int j = 0; j < 9; j++)
        {
            if (_options[x, j].Count == 1) values.Add(_options[x, j].First());
        }

        //Column
        for (int i = 0; i < 9; i++)
        {
            if (_options[i, y].Count == 1) values.Add(_options[i, y].First());
        }

        //Set
        int k = x / 3;
        int l = y / 3;

        for (int i = k * 3; i < (k + 1) * 3; i++)
        {
            for (int j = l * 3; j < (l + 1) * 3; j++)
            {
                if (_options[i, j].Count == 1) values.Add(_options[i, j].First());
            }
        }

        values = values.Distinct().ToList();

        _options[x, y] = new List<char>();
        _options[x, y].AddRange(digits.Where(d => !values.Contains(d)));
    }

    public void CalculateOptions()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count != 1)
                {
                    CalculateCellOptions(i, j);
                }
            }
        }
    }

    public bool ValidateBoard()
    {
        var symbols = new HashSet<char>();

        //Validate rows
        for (int i = 0; i < 9; i++)
        {
            symbols.Clear();

            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count == 1)
                {
                    if (symbols.Contains(_options[i, j][0])) return false;
                    symbols.Add(_options[i, j][0]);
                }
            }
        }

        //Validate columns
        for (int i = 0; i < 9; i++)
        {
            symbols.Clear();

            for (int j = 0; j < 9; j++)
            {
                if (_options[j, i].Count == 1)
                {
                    if (symbols.Contains(_options[j, i][0])) return false;
                    symbols.Add(_options[j, i][0]);
                }
            }
        }

        //Validate sets
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                symbols.Clear();

                for (int k = 0; k < 3; k++)
                {
                    for (int l = 0; l < 3; l++)
                    {
                        if (_options[i * 3 + k, j * 3 + l].Count == 1)
                        {
                            if (symbols.Contains(_options[i * 3 + k, j * 3 + l][0])) return false;
                            symbols.Add(_options[i * 3 + k, j * 3 + l][0]);
                        }
                    }
                }
            }
        }

        return true;
    }

    public List<char> GetOptions(int x, int y) => _options[x, y];

    public void SetOption(int x, int y, char value)
    {
        _options[x, y] = new List<char> { value };
    }

    public void DisplayBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (_options[i, j].Count == 1) Console.Write(_options[i, j][0]);
                else Console.Write('.');
                Console.Write(' ');
            }
            Console.WriteLine();
        }
        if (!ValidateBoard())
        {
            Console.WriteLine("Board invalid!");
        }
        Console.WriteLine();
    }

    public void CheckBoard()
    {
        DisplayBoard();
        CalculateOptions();
    }
}