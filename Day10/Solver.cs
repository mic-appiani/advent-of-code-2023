namespace Day10;

public class Solver
{
    private int _solution;
    private int _startRow = -1;
    private int _startCol = -1;

    private List<List<char>> _map = new List<List<char>>();

    public int Solve(int part)
    {
        using var sr = new StreamReader("sample_1.txt");

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

        PrintMap();
        Pipe pipe = FindEntryPipe(_startRow, _startCol);
        Console.WriteLine();
        // pick one direction and follow it
        // start from S and detect connected cells
        // assumption: is a continuous loop
        // continue until S is hit
        // divide steps by 2 (round up to next int)

        return _solution;
    }

    private Pipe FindEntryPipe(int startRow, int startCol)
    {
        // for now let's ignore boundaries because i'm feeling lucky
        // check connections up
        var pipe = new Pipe(startRow - 1, startCol, _map);

        if (pipe.CanConnectTo(startRow, startCol))
        {
            return pipe;
        }

        pipe = new Pipe(startRow + 1, startCol, _map);

        if (pipe.CanConnectTo(startRow, startCol))
        {
            return pipe;
        }

        pipe = new Pipe(startRow, startCol - 1, _map);

        if (pipe.CanConnectTo(startRow, startCol))
        {
            return pipe;
        }

        pipe = new Pipe(startRow, startCol + 1, _map);

        if (pipe.CanConnectTo(startRow, startCol))
        {
            return pipe;
        }

        throw new Exception("The location cannot connect to any surrounding pipe");
    }

    private void PrintMap()
    {
        for (int row = 0; row < _map.Count; row++)
        {
            var line = "";
            for (int col = 0; col < _map[row].Count; col++)
            {
                line += _map[row][col];
            }

            Console.WriteLine(line);
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
        var direction = GetDirectionRelativeTo(row, col);
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
        Direction entranceDirection = GetDirectionRelativeTo(entryRow, entryCol);

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
    private Direction GetDirectionRelativeTo(int entryRow, int entryCol)
    {
        if (entryRow == Row - 1)
        {
            return Direction.North;
        }

        if (entryRow == Row + 1)
        {
            return Direction.South;
        }

        if (entryCol == Row - 1)
        {
            return Direction.West;
        }

        if (entryCol == Row + 1)
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