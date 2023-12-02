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

            var numberString = string.Empty;
            var left = Utils.SearchNumber(input);
            numberString += left;

            var right = Utils.SearchNumber(input, reverse: true);
            numberString += right;

            // convert to int
            solution += int.Parse(numberString);
        }

        return solution;
    }

    private bool IsDigit(char c)
    {
        return c >= 48 && c <= 57;
    }
}