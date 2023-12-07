using System.Text.RegularExpressions;

namespace Day05;

public class Part2 : IDay5Solver
{
    private readonly List<long[]> _seedRanges = new();
    private long _solution = long.MaxValue;

    private const int SeedRangeStart = 0;
    private const int SeedRangeLength = 1;

    public void ParseSeeds(string input)
    {
        string pattern = "([0-9]+ [0-9]+)";
        var regex = new Regex(pattern);

        foreach (Match match in regex.Matches(input!))
        {
            var split = match.Value.Split(' ').ToArray();
            var range = split.Select(x => long.Parse(x)).ToArray();
            _seedRanges.Add(range);
        }
    }

    public long Solve(Dictionary<MapType, List<long[]>> maps)
    {
        foreach (var range in _seedRanges)
        {
            Console.WriteLine($"range {_seedRanges.IndexOf(range)} of {_seedRanges.Count}");
            var initial = range[SeedRangeStart];
            var final = range[SeedRangeStart] + range[SeedRangeLength];
            for (long i = initial; i < final; i++)
            {
                if (i % 1_000_000 == 0)
                {
                    Console.WriteLine($"progress :{(double)(i - initial) / (final - initial)}");
                }

                var result = Solver.MapSeedToLocation(i, maps);

                if (result < _solution)
                {
                    _solution = result;
                }
            }
        }

        return _solution;
    }
}