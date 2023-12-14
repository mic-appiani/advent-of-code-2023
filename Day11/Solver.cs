namespace Day11;

public class Solver
{
    private int _solution;
    private List<List<char>> _map = [];
    private Dictionary<int, int[]> _galaxies = new();

    public int Solve(int part)
    {
        using var sr = new StreamReader("sample_1.txt");

        int rowNumber = 0;
        while (!sr.EndOfStream)
        {
            var row = sr.ReadLine();

            var isEmpty = true;
            var galaxyCounter = 0;
            _map.Add([]);

            for (var col = 0; col < row!.Length; col++)
            {
                if (row[col] == '#')
                {
                    isEmpty = false;
                    _galaxies[galaxyCounter] = new[] { rowNumber, col };
                    galaxyCounter++;
                }

                _map[rowNumber].Add(row[col]);
            }


            if (isEmpty)
            {
                var emptyRow = Enumerable.Range(0, row.Length).Select(x => '.').ToList();
                _map.Add(emptyRow);
                rowNumber++;
            }

            rowNumber++;
        }

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

        PrintMap();
        
        // calculate the shortest distance between each pair of galaxies, with no duplicate pairs
        // 1-2 is the same as 2-1

        // the solution is the sum of all the distances
        return _solution;
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