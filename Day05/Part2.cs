using System.Linq.Expressions;
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
        var locs = maps.GetOrderedLocations();

        // using reverse mapping from the closes location
        // todo: maybe some day i will try to understand the better solution
        foreach (var range in locs)
        {
            for (long i = range[0]; i <= range[1]; i++)
            {
                var seed = maps.MapLocationToSeed(i);

                if (IsInSeeds(seed))
                {
                    return i;
                }
            }
        }

        return _solution;
    }

    private bool IsInSeeds(long seed)
    {
        // var range = _seedRanges.Single(r => r.Key <= seed && seed <= r.Value);
        return _seedRanges.Any(r => r.Key <= seed && seed <= r.Value);
    }
}