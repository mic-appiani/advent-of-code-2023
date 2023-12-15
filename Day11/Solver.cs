using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Day11;

public class Solver
{
    private long _solution;
    private List<List<char>> _map = [];
    private Dictionary<int, int[]> _galaxies = new();
    private List<int> _emptyRows = [];
    private List<int> _emptyCols = [];

    public long Solve(int part)
    {
        using var sr = new StreamReader("input.txt");

        while (!sr.EndOfStream)
        {
            var row = sr.ReadLine();
            var rowList = row!.ToList();
            _map.Add(rowList);

            if (rowList.All(c => c == '.'))
            {
                _emptyRows.Add(_map.Count - 1);
            }
        }

        HandleEmptyColumns();

        DetectGalaxies();
        CalculateDistances(part);

        return _solution;
    }

    private void DetectGalaxies()
    {
        var galaxyId = 1;
        for (int row = 0; row < _map.Count; row++)
        {
            for (int col = 0; col < _map[0].Count; col++)
            {
                if (_map[row][col] == '#')
                {
                    _galaxies[galaxyId] = new[] { row, col };
                    galaxyId++;
                }
            }
        }
    }

    private void HandleEmptyColumns()
    {
        for (int col = 0; col < _map[0].Count; col++)
        {
            var isEmpty = true;
            for (int row = 0; row < _map.Count; row++)
            {
                if (_map[row][col] == '#')
                {
                    isEmpty = false;
                    break;
                }
            }

            if (isEmpty)
            {
                _emptyCols.Add(col);
            }
        }
    }

    private void CalculateDistances(int part)
    {
        var pairs = 0;
        for (int i = 1; i < _galaxies.Keys.Max(); i++)
        {
            for (int j = i + 1; j <= _galaxies.Keys.Max(); j++)
            {
                var from = _galaxies[i];
                var to = _galaxies[j];


                var deltaY = Math.Abs(to[0] - from[0]);
                var deltaX = Math.Abs(to[1] - from[1]);

                var emptyRows = _emptyRows.Count(x => from[0] < x && to[0] > x);
                var emptyCols = _emptyCols.Count(x => (from[1] < x && to[1] > x) ||
                                                      (to[1] < x && from[1] > x));
                var emptySpaceMultiplier = part == 1 ? 1 : 999_999;
                var dist = deltaY + deltaX + (emptyRows + emptyCols) * emptySpaceMultiplier;
                pairs++;
                _solution += dist;
            }
        }

        Console.WriteLine($"Pairs: {pairs}");
    }
}