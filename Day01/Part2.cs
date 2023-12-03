namespace Day01;

public class Part2
{
    public int Solve()
    {
        string? input;
        int solution = 0;

        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            if (input is null)
            {
                break;
            }

            var res = Utils.ScanNumbers(input);

            var numberToParse = string.Empty;
            numberToParse += res.First();
            numberToParse += res.Last();

            // convert to int
            solution += int.Parse(numberToParse);
        }

        return solution;
    }

    private bool IsDigit(char c)
    {
        return c >= 48 && c <= 57;
    }
}