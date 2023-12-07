using System.Text.RegularExpressions;

namespace Day05;

public class Part2 : IDay5Solver
{
    // seed ranges are first and last seed, inclusive
    private readonly SortedDictionary<long, long> _seedRanges = new();
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
            _seedRanges.Add(range[SeedRangeStart], 
                range[SeedRangeStart] + range[SeedRangeLength] - 1);
        }
    }

    public long Solve(FarmingMap maps)
    {
        // iterate the location ranges starting from the one with the lowest start value
        // find the seed by reverse mapping
        // if the seed exists in the seed ranges, return the location
        

        return _solution;
    }
}