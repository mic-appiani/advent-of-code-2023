namespace Day01;

public static class Utils
{
    public static bool IsDigit(char c)
    {
        return c >= 48 && c <= 57;
    }

    public static char SearchNumber(string input, bool reverse = false)
    {
        var startIndex = reverse ? input.Length - 1 : 0;
        Func<int, bool> condition = i => reverse ? i >= 0 : i < input.Length ;
        Func<int, int> iterator = i => reverse ? i - 1 : i + 1;

        for (int i = startIndex; condition(i); i = iterator(i))
        {
            if (IsDigit(input[i]))
            {
                return input[i];
            }
        }

        return (char)0;
    }

    public static List<Digit> Digits = new()
    {
        new() { Word = "zero", Number = "0" },
        new() { Word = "one", Number = "1" },
        new() { Word = "two", Number = "2" },
        new() { Word = "three", Number = "3" },
        new() { Word = "four", Number = "4" },
        new() { Word = "five", Number = "5" },
        new() { Word = "six", Number = "6" },
        new() { Word = "seven", Number = "7" },
        new() { Word = "eight", Number = "8" },
        new() { Word = "nine", Number = "9" },
    };

    public static List<Digit> DigitsReversed = new()
    {
        new() { Word = "orez", Number = "0" },
        new() { Word = "eno", Number = "1" },
        new() { Word = "owt", Number = "2" },
        new() { Word = "eerht", Number = "3" },
        new() { Word = "ruof", Number = "4" },
        new() { Word = "evif", Number = "5" },
        new() { Word = "xis", Number = "6" },
        new() { Word = "neves", Number = "7" },
        new() { Word = "thgie", Number = "8" },
        new() { Word = "enin", Number = "9" },
    };
}