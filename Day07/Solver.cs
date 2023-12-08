using System.Text.RegularExpressions;

public class Solver
{
    private int _solution = 0;

    public int Solve(int part)
    {
        using var sr = new StreamReader("input.txt");
        string? input;
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();
        }

        return _solution;
    }
}