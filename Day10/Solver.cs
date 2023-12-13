namespace Day10;

public class Solver
{
    private int _solution;
    private int _startRow = -1;
    private int _startCol = -1;

    private List<List<char>> _map = [];
    private bool[,] _visited;

    public int Solve(int part)
    {
        using var sr = new StreamReader("sample_2.txt");

        var currentRow = -1;
        while (!sr.EndOfStream)
        {
            var input = sr.ReadLine();
            currentRow++;

            var row = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == 'S')
                {
                    _startRow = currentRow;
                    _startCol = i;
                }

                row.Add(input[i]);
            }

            _map.Add(row);
        }

        _visited = new bool[_map.Count, _map[0].Count];
        Pipe? nextPipe = FindConnection(_startRow, _startCol);
        var steps = 0;

        while (nextPipe is not null)
        {
            steps++;
            _visited[nextPipe.Row, nextPipe.Col] = true;
            // must track visited pipes
            nextPipe = FindConnection(nextPipe.Row, nextPipe.Col);
        }

        PrintMap();
        _solution = (int)Math.Ceiling((double)steps / 2);
        return _solution;
    }

    private Pipe? FindConnection(int startRow, int startCol)
    {
        // for now let's ignore boundaries because i'm feeling lucky
        // check connections up
        Pipe pipe;
        if (startRow > 0)
        {
            pipe = new Pipe(startRow - 1, startCol, _map);

            if (pipe.CanConnectTo(startRow, startCol) && !_visited[pipe.Row, pipe.Col])
            {
                return pipe;
            }
        }

        if (startRow < _map.Count - 1)
        {
            pipe = new Pipe(startRow + 1, startCol, _map);

            if (pipe.CanConnectTo(startRow, startCol) && !_visited[pipe.Row, pipe.Col])
            {
                return pipe;
            }
        }

        if (startCol > 0)
        {
            pipe = new Pipe(startRow, startCol - 1, _map);

            if (pipe.CanConnectTo(startRow, startCol) && !_visited[pipe.Row, pipe.Col])
            {
                return pipe;
            }
        }

        if (startCol < _map[0].Count - 1) ;
        {
            pipe = new Pipe(startRow, startCol + 1, _map);

            if (pipe.CanConnectTo(startRow, startCol) && !_visited[pipe.Row, pipe.Col])
            {
                return pipe;
            }
        }

        return null;
    }

    private void PrintMap()
    {
        for (int row = 0; row < _map.Count; row++)
        {
            for (int col = 0; col < _map[row].Count; col++)
            {
                Console.ForegroundColor = _visited[row, col] ? ConsoleColor.Red : ConsoleColor.Gray;

                Console.Write(_map[row][col]);
            }

            Console.WriteLine();
        }
    }
}

public class Pipe
{
    public Pipe(int row, int col, List<List<char>> map)
    {
        Row = row;
        Col = col;
        Symbol = map[row][col];
    }

    public char Symbol { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public bool CanConnectTo(int row, int col)
    {
        var direction = GetRelativePosition(row, col);
        return Symbol switch
        {
            '|' => direction == Direction.South || direction == Direction.North,
            '-' => direction == Direction.East || direction == Direction.West,
            'L' => direction == Direction.East || direction == Direction.North,
            'J' => direction == Direction.West || direction == Direction.North,
            '7' => direction == Direction.West || direction == Direction.South,
            'F' => direction == Direction.East || direction == Direction.South,
            _ => false,
        };
    }

    public int[] GetExit(int entryRow, int entryCol)
    {
        Direction entranceDirection = GetRelativePosition(entryRow, entryCol);

        if (Symbol == '|')
        {
            if (entranceDirection is Direction.East or Direction.West)
            {
                throw new ArgumentException("Illegal arguments");
            }

            int exitRow = entranceDirection == Direction.South ? Row - 1 : Row + 1;
            return new[] { exitRow, entryCol };
        }

        if (Symbol == '-')
        {
            if (entranceDirection is Direction.North or Direction.South)
            {
                throw new ArgumentException("Illegal arguments");
            }

            int exitCol = entranceDirection == Direction.East ? Col - 1 : Col + 1;
            return new[] { entryRow, exitCol };
        }

        if (Symbol == 'L')
        {
            if (entranceDirection is Direction.West or Direction.South)
            {
                throw new ArgumentException("Illegal arguments");
            }

            var exitCol = entranceDirection == Direction.North ? Col + 1 : Col;
            var exitRow = entranceDirection == Direction.North ? Row : Row - 1;
            return new[] { exitRow, exitCol };
        }

        if (Symbol == 'J')
        {
            if (entranceDirection is Direction.East or Direction.South)
            {
                throw new ArgumentException("Illegal arguments");
            }

            var exitCol = entranceDirection == Direction.North ? Col - 1 : Col;
            var exitRow = entranceDirection == Direction.North ? Row : Row - 1;
            return new[] { exitRow, exitCol };
        }

        if (Symbol == '7')
        {
            if (entranceDirection is Direction.East or Direction.North)
            {
                throw new ArgumentException("Illegal arguments");
            }

            var exitCol = entranceDirection == Direction.South ? Col - 1 : Col;
            var exitRow = entranceDirection == Direction.South ? Row : Row + 1;
            return new[] { exitRow, exitCol };
        }

        if (Symbol == 'F')
        {
            if (entranceDirection is Direction.West or Direction.North)
            {
                throw new ArgumentException("Illegal arguments");
            }

            var exitCol = entranceDirection == Direction.South ? Col + 1 : Col;
            var exitRow = entranceDirection == Direction.South ? Row : Row + 1;
            return new[] { exitRow, exitCol };
        }

        throw new ArgumentOutOfRangeException(nameof(Symbol));
    }

    /// <summary>
    /// Does not support diagonals
    /// </summary>
    /// <param name="entryRow"></param>
    /// <param name="entryCol"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private Direction GetRelativePosition(int entryRow, int entryCol)
    {
        if (entryRow == Row - 1)
        {
            return Direction.North;
        }

        if (entryRow == Row + 1)
        {
            return Direction.South;
        }

        if (entryCol == Col - 1)
        {
            return Direction.West;
        }

        if (entryCol == Col + 1)
        {
            return Direction.East;
        }

        throw new ArgumentException("Invalid entry direction");
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}