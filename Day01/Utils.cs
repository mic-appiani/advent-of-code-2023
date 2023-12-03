namespace Day01;

public static class Utils
{
    public static bool IsDigit(char c)
    {
        return c >= 48 && c <= 57;
    }

    /// this method must get a start index and input string as params and returns:
    /// success status
    /// the first valid number
    /// the last valid scanned index
    public static List<char> ScanNumbers(string input)
    {
        List<char> list = new();
        var startIdx = 0;

        while (startIdx < input.Length)
        {
            var result = FindNumber(input, startIdx);

            if (result.Success)
            {
                list.Add(result.CharacterNumber);
            }

            // was supposed to be clever and skip to save time, but some numbers overlap..
            // ideal solution is to do a forward then a reverse scan only keeping the 1st match.
            startIdx++;
        }

        return list;
    }

    /// <summary>
    /// Scans from i letter by letter. If there is a valid match, it is returned.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private static SearchResult FindNumber(string input, int startIdx)
    {
        for (int i = startIdx; i < input.Length; i++)
        {
            if (IsDigit(input[i]))
            {
                return new SearchResult
                {
                    Success = true,
                    CharacterNumber = input[i],
                    NextIndex = i + 1,
                };
            }

            var matches = Enumerable.Range(0, 10).ToList();
            var offset = 0;
            while (matches.Count > 1)
            {
                if (i + offset >= input.Length)
                {
                    return new SearchResult { NextIndex = i + 1 };
                }

                matches = Matches(input[i + offset], offset, matches);
                offset++;
            }

            if (matches.Count == 1)
            {
                var match = Digits[matches.First()];

                SearchResult res = LinearScan(input, i, match.Word);
                if (res.Success)
                {
                    res.CharacterNumber = match.Character;
                    return res;
                }

                return new SearchResult { NextIndex = i + 1 };
            }

            if (matches.Count == 0)
            {
                return new SearchResult { NextIndex = i + 1 };
            }
        }

        return new SearchResult { NextIndex = input.Length };
    }

    private static SearchResult LinearScan(string input, int startIdx, string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            if (i + startIdx >= input.Length)
            {
                return new SearchResult { Success = false };
            }

            if (!word[i].Equals(input[startIdx + i]))
            {
                return new SearchResult { Success = false };
            }
        }

        return new SearchResult
        {
            Success = true,
            NextIndex = startIdx + word.Length,
        };
    }

    private static List<int> Matches(
        char character,
        int offset,
        List<int> idxToCheck)
    {
        var reference = Digits;
        var matches = new List<int>();

        for (int i = 0; i < idxToCheck.Count; i++)
        {
            var word = reference[idxToCheck[i]].Word;
            if (offset < word.Length && character == word[offset])
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
}

internal record SearchResult
{
    public bool Success { get; set; }
    public char CharacterNumber { get; set; }
    public int NextIndex { get; set; }
}