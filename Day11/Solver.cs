using System.Diagnostics;

namespace Day11;

public class Solver
{
    private long _solution;
    private List<List<char>> _map = [];
    private Dictionary<int, int[]> _galaxies = new();

    public long Solve(int part)
    {
        using var sr = new StreamReader("input.txt");

        while (!sr.EndOfStream)
        {
            var row = sr.ReadLine();
            // todo: for part 2, keep track of the empty row locations
            // do not expand the map manualy, instead,
            // when calculating, use a variable to add the extra space for each empty row/col
            var rowList = row!.ToList();
            _map.Add(rowList);

            if (rowList.All(c => c == '.'))
            {
                _map.Add(rowList);
            }

        }

        HandleEmptyColumns();

        // PrintMap();
        DetectGalaxies();
        CalculateDistances();

        // the solution is the sum of all the distances
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
                }
            }

            if (isEmpty)
            {
                InsertEmptyColumnLeft(col);
                col++;
            }
        }
    }

    private void CalculateDistances()
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

                var dist = deltaY + deltaX;
                pairs++;
                _solution += dist;
            }
        }

        Console.WriteLine($"Pairs: {pairs}");
    }

    private void PrintMap()
    {
        for (int row = 0; row < _map.Count; row++)
        {
            for (int col = 0; col < _map[0].Count; col++)
            {
                Console.Write(_map[row][col]);
            }

            Console.WriteLine();
        }
    }

    private void InsertEmptyColumnLeft(int referenceColumn)
    {
        for (int row = 0; row < _map.Count; row++)
        {
            _map[row].Insert(referenceColumn, '.');
        }
    }
}