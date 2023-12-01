namespace Day01.Part1;

public class Part1
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

            // find first digit
            for (int i = 0; i < input.Length; i++)
            {
                if (IsDigit(input[i]))
                {
                    numberString += input[i];
                    break;
                }
            }

            // find last digit
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (IsDigit(input[i]))
                {
                    numberString += input[i];
                    break;
                }
            }

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