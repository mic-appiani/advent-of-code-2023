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
        Func<int, bool> condition = i => reverse ? i >= 0 : i < input.Length;
        Func<int, int> iterator = i => reverse ? i - 1 : i + 1;

        for (int i = startIndex; condition(i); i = iterator(i))
        {
            return DoSearch(input, i, reverse);
        }

        return (char)0;
    }

    private static char DoSearch(string input, int startIdx, bool reverse)
    {
        Func<int, bool> condition = i => reverse ? i >= 0 : i < input.Length;
        Func<int, int> iterator = i => reverse ? i - 1 : i + 1;

        var digits = reverse ? DigitsReversed : Digits;
        var toCheck = Enumerable.Range(0, 10).ToList();

        for (int i = startIdx; condition(i); i = iterator(i))
        {
            if (IsDigit(input[i]))
            {
                return input[i];
            }

            startIdx = i;
            var matches = Matches(input[i],
                reverse ? startIdx - i : i - startIdx,
                toCheck, digits);

            if (matches.Count == 1)
            {
                // make sure that this match is real..
                var matchingIdx = matches.First();

                // use the non reversed always
                var reference = Digits[matchingIdx];

                var len = reference.Word.Length;

                var start = reverse ? i - len - 1 : i;

                if (start < 0 || start + len > input.Length)
                {
                    continue;
                }
                
                var substring = input.Substring(start, len);

                if (substring.Equals(reference.Word))
                {
                    return digits[matchingIdx].Character;
                }
            }

            if (matches.Count > 1)
            {
                toCheck = matches;
            }
        }

        throw new Exception("Why did this happen?");
    }

    private static List<int> Matches(char character,
        int idx,
        List<int> idxToCheck,
        List<Digit> reference)
    {
        var matches = new List<int>();

        for (int i = 0; i < idxToCheck.Count; i++)
        {
            var word = reference[idxToCheck[i]].Word;
            if (idx < word.Length && character == word[idx])
            {
                matches.Add(idxToCheck[i]);
            }
        }

        return matches;
    }

    public static List<Digit> Digits = new()
    {
        new() { Word = "zero", Character = '0' },
        new() { Word = "one", Character = '1' },
        new() { Word = "two", Character = '2' },
        new() { Word = "three", Character = '3' },
        new() { Word = "four", Character = '4' },
        new() { Word = "five", Character = '5' },
        new() { Word = "six", Character = '6' },
        new() { Word = "seven", Character = '7' },
        new() { Word = "eight", Character = '8' },
        new() { Word = "nine", Character = '9' },
    };

    public static List<Digit> DigitsReversed = new()
    {
        new() { Word = "orez", Character = '0' },
        new() { Word = "eno", Character = '1' },
        new() { Word = "owt", Character = '2' },
        new() { Word = "eerht", Character = '3' },
        new() { Word = "ruof", Character = '4' },
        new() { Word = "evif", Character = '5' },
        new() { Word = "xis", Character = '6' },
        new() { Word = "neves", Character = '7' },
        new() { Word = "thgie", Character = '8' },
        new() { Word = "enin", Character = '9' },
    };
}