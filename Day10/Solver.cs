using System.Xml.Schema;

namespace Day10;

public class Solver
{
    private int _solution;
    private int _startRow = -1;
    private int _startCol = -1;

    private List<List<char>> _map = [];
    private bool[,] _visited;
    private char[,] _boundaries;

    public int Solve(int part)
    {
        using var sr = new StreamReader("input.txt");

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
        _map[_startRow][_startCol] = 'L';
        var nextPipe = Pipe.SetInitial(_startRow, _startCol, 'L');
        var steps = 0;

        while (nextPipe != null && !_visited[nextPipe.Row, nextPipe.Col])
        {
            steps++;
            _visited[nextPipe.Row, nextPipe.Col] = true;
            nextPipe = FindConnection(nextPipe);
        }

        if (part == 1)
        {
            PrintMapPipes();
            _solution = (int)Math.Ceiling((double)(steps - 1) / 2);
            return _solution;
        }

        // For part 2, i had to go look at hints.
        // Pick's Theorem / shoelace formula and the "scanline" approach were suggested
        // I'm trying the latter. Heavily explained on a reddit thread:
        // https://www.reddit.com/r/adventofcode/comments/18f1sgh/2023_day_10_part_2_advise_on_part_2/
        if (part == 2)
        {
            _boundaries = new char[_map.Count, _map[0].Count];

            for (int row = 0; row < _map.Count; row++)
            {
                var inside = false;
                for (int col = 0; col < _map[row].Count; col++)
                {
                    var tile = _map[row][col];
                    char lastCorner = '?';

                    if (_visited[row, col])
                    {
                        if ("|F7".Contains(tile))
                        {
                            inside = !inside;
                        }

                        _boundaries[row, col] = tile;
                    }
                    else
                    {
                        if (inside)
                        {
                            _boundaries[row, col] = 'I';
                            _solution++;
                        }
                        else
                        {
                            _boundaries[row, col] = 'O';
                        }
                    }
                }
            }

            PrintBoundaries();
            return _solution;
        }

        return -1;
    }


    private Pipe? FindConnection(Pipe sourcePipe)
    {
        // for now let's ignore boundaries because i'm feeling lucky
        // check connections up
        Pipe destPipe;
        if (sourcePipe.Row > 0)
        {
            destPipe = new Pipe(sourcePipe.Row - 1, sourcePipe.Col, _map);

            if (Connectible(sourcePipe, destPipe))
            {
                return destPipe;
            }
        }

        if (sourcePipe.Row < _map.Count - 1)
        {
            destPipe = new Pipe(sourcePipe.Row + 1, sourcePipe.Col, _map);

            if (Connectible(sourcePipe, destPipe))
            {
                return destPipe;
            }
        }

        if (sourcePipe.Col > 0)
        {
            destPipe = new Pipe(sourcePipe.Row, sourcePipe.Col - 1, _map);

            if (Connectible(sourcePipe, destPipe))
            {
                return destPipe;
            }
        }

        if (sourcePipe.Col < _map[0].Count - 1) ;
        {
            destPipe = new Pipe(sourcePipe.Row, sourcePipe.Col + 1, _map);

            if (Connectible(sourcePipe, destPipe))
            {
                return destPipe;
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if the pipes can connect to each other and if the pipe has not been visited 
    /// </summary>
    /// <param name="sourcePipe"></param>
    /// <param name="destPipe"></param>
    /// <returns></returns>
    private bool Connectible(Pipe sourcePipe, Pipe destPipe)
    {
        return destPipe.CanConnectTo(sourcePipe) &&
               sourcePipe.CanConnectTo(destPipe) &&
               !_visited[destPipe.Row, destPipe.Col];
    }

    private void PrintBoundaries()
    {
        for (int row = 0; row < _map.Count; row++)
        {
            for (int col = 0; col < _map[row].Count; col++)
            {

                if (_visited[row, col])
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                
                var tile = _boundaries[row,col];
                if (tile == 'I')
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else if (tile == 'O')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                
                Console.Write(_boundaries[row, col]);
            }

            Console.WriteLine();
        }
    }

    private void PrintMapPipes()
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

    private Pipe(int row, int col, char symbol)
    {
        Row = row;
        Col = col;
        Symbol = symbol;
    }

    public char Symbol { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }

    public bool CanConnectTo(Pipe pipe)
    {
        var pos = PositionOf(pipe.Row, pipe.Col);

        return Symbol switch
        {
            '|' => pos == Direction.South || pos == Direction.North,
            '-' => pos == Direction.East || pos == Direction.West,
            'L' => pos == Direction.East || pos == Direction.North,
            'J' => pos == Direction.West || pos == Direction.North,
            '7' => pos == Direction.West || pos == Direction.South,
            'F' => pos == Direction.East || pos == Direction.South,
            _ => false,
        };
    }

    public int[] GetExit(int entryRow, int entryCol)
    {
        Direction entranceDirection = PositionOf(entryRow, entryCol);

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
    private Direction PositionOf(int entryRow, int entryCol)
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

    public static Pipe SetInitial(int startRow, int startCol, char c)
    {
        return new Pipe(startRow, startCol, c);
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}