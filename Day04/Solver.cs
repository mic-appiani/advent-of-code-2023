namespace Day04;

public class Solver
{
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
        }

        return solution;
    }
}