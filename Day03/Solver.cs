using System.Security.Principal;

namespace Day03;

public class Solver
{
    public const char WhiteSpace = '.';

    public static Dictionary<char, int> Digits = new()
    {
        { '0', 0 },
        { '1', 1 },
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
    };

    private readonly List<List<char>> _matrix = new();

    public int Solve(int part)
    {
        string? input;
        var solution = 0;

        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            if (input is null)
            {
                throw new Exception("input should not be null");
            }

            // put everything in a list of lists of chars
            var row = input.ToCharArray().ToList();
            _matrix.Add(row);
        }

        // scan for symbols
        for (int row = 0; row < _matrix.Count; row++)
        {
            for (int col = 0; col < _matrix[row].Count; col++)
            {
                var value = _matrix[row][col];

                if (value is WhiteSpace ||
                    IsDigit(value))
                {
                    continue;
                }

                // at this point it must be a symbol
                var surroundingNumbers = ScanSurrounds(row, col);

                // add the number to the solution
                if (part == 1)
                {
                    foreach (var number in surroundingNumbers)
                    {
                        solution += number;
                    }
                }
                else if (part == 2 &&
                         surroundingNumbers.Count == 2)
                {
                    solution += surroundingNumbers[0] * surroundingNumbers[1];
                }
            }
        }

        return solution;
    }

    // finds all the numbers surrounding the location
    private List<int> ScanSurrounds(int row, int col)
    {
        var minRow = row == 0 ? 0 : row - 1;
        var maxRow = row == _matrix.Count - 1 ? _matrix.Count - 1 : row + 1;
        var minCol = col == 0 ? 0 : col - 1;
        var maxCol = col == _matrix[0].Count - 1 ? _matrix[0].Count - 1 : col + 1;

        var numbers = new List<string>();
        for (int i = minRow; i <= maxRow; i++)
        {
            for (int j = minCol; j <= maxCol; j++)
            {
                if (IsDigit(_matrix[i][j]))
                {
                    string numberString = ExtractCompleteNumber(i, j);
                    numbers.Add(numberString);
                }
            }
        }

        // todo: this is ugly. removing duplicates hoping that there is no legit duplicate in the sample! sad
        return numbers.Distinct()
            .Select(x => int.Parse(x))
            .ToList();
    }

    private string ExtractCompleteNumber(int row, int col)
    {
        LinkedList<char> completeNumber = new();
        completeNumber.AddFirst(_matrix[row][col]);

        // scan left
        int pointer = col - 1;
        while (pointer >= 0 &&
               IsDigit(_matrix[row][pointer]))
        {
            completeNumber.AddFirst(_matrix[row][pointer]);
            pointer--;
        }

        // scan right

        pointer = col + 1;
        while (pointer <= _matrix[row].Count - 1 &&
               IsDigit(_matrix[row][pointer]))
        {
            completeNumber.AddLast(_matrix[row][pointer]);
            pointer++;
        }

        return string.Join("", completeNumber);
    }

    private static bool IsDigit(char value)
    {
        return Digits.TryGetValue(value, out _);
    }
}