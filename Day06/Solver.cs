public class Solver
{
    private int _solution = 0;

    public int Solve(int part)
    {
        string? input;

        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            if (input is null)
            {
                throw new Exception("input should not be null");
            }
        }

        return _solution;
    }
}