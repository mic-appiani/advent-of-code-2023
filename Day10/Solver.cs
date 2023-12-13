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
        // start from S and detect connected cells
        // assumption: is a continuous loop
        // continue until S is hit
        // divide steps by 2 (round up to next int)
        
        return _solution;
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