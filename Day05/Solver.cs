using System.Diagnostics;
using System.Text.RegularExpressions;

public class Solver
{
    private long _solution = long.MaxValue;
    private const int DestinationRangeStart = 0;
    private const int SourceRangeStart = 1;
    private const int RangeLength = 2;

    public long Solve(int part)
    {
        string? input;

        using var sr = new StreamReader("input.txt");
        input = sr.ReadLine();

        string pattern = "seeds: (.+)";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(input!);
        var seeds = match.Groups[1].Value.Split(' ').Select(s => long.Parse(s));

        AdvanceToLabel("seed-to-soil map:", sr);
        var seedToSoil = MapSection(sr);
        AdvanceToLabel("soil-to-fertilizer map:", sr);
        var soilToFertilizer = MapSection(sr);
        AdvanceToLabel("fertilizer-to-water map:", sr);
        var fertilizerToWater = MapSection(sr);
        AdvanceToLabel("water-to-light map:", sr);
        var waterToLight = MapSection(sr);
        AdvanceToLabel("light-to-temperature map:", sr);
        var lightToTemp = MapSection(sr);
        AdvanceToLabel("temperature-to-humidity map:", sr);
        var tempToHumidity = MapSection(sr);
        AdvanceToLabel("humidity-to-location map:", sr);
        var humidityToLocation = MapSection(sr);

        // read maps in sections
        // compute calculation necessary to move from map to map
        // (create helper methods that takes and input value and gives the destination directly,
        // to be used in the next map, and so on, to the location number)
        // return the smallest locaton number (day 1 solution)
        foreach (var seed in seeds)
        {
            var result = GetDestination(seed, seedToSoil);
            result = GetDestination(result, soilToFertilizer);
            result = GetDestination(result, fertilizerToWater);
            result = GetDestination(result, waterToLight);
            result = GetDestination(result, lightToTemp);
            result = GetDestination(result, tempToHumidity);
            result = GetDestination(result, humidityToLocation);

            if (result < _solution)
            {
                _solution = result;
            }
        }

        return _solution;
    }

    private long GetDestination(long i, List<long[]> referenceMap)
    {
        // find the map where i is. Assume only one match
        var map = referenceMap
            .SingleOrDefault(x => x[SourceRangeStart] <= i &&
                                  i <= x[SourceRangeStart] + x[RangeLength] - 1);

        if (map is null)
        {
            Console.WriteLine("no map");
            return i;
        }

        var offset = i - map[SourceRangeStart];

        return map[DestinationRangeStart] + offset;
    }

    private List<long[]> MapSection(StreamReader sr)
    {
        var list = new List<long[]>();

        while (true)
        {
            var input = sr.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return list;
            }

            var arr = input.Split(' ').Select(x => long.Parse(x)).ToArray();
            list.Add(arr);
        }
    }

    private void AdvanceToLabel(string label, StreamReader sr)
    {
        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            if (line!.Equals(label))
            {
                return;
            }
        }

        throw new Exception("Label not found");
    }
}