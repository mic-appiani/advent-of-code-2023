using System.Text.RegularExpressions;

namespace Day05;

public class Part1 : IDay5Solver
{
    private List<long> _seeds = default!;
    private long _solution = long.MaxValue;

    public void ParseSeeds(string input)
    {
        string pattern = "seeds: (.+)";
        var regex = new Regex(pattern);
        var match = regex.Match(input!);
        _seeds = match.Groups[1].Value.Split(' ')
            .Select(s => long.Parse(s))
            .ToList();
    }

    public long Solve(FarmingMap maps)
    {
        foreach (var seed in _seeds)
        {
            var result = maps.MapSeedToLocation(seed);

            if (result < _solution)
            {
                _solution = result;
            }
        }

        return _solution;
    }
}