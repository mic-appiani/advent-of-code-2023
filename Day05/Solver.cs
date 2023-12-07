using System.Diagnostics;
using System.Text.RegularExpressions;
using Day05;

public class Solver
{
    private const int DestinationRangeStart = 0;
    private const int SourceRangeStart = 1;
    private const int RangeLength = 2;

    public long Solve(IDay5Solver helper)
    {
        string? input;

        using var sr = new StreamReader("input.txt");
        input = sr.ReadLine();

        helper.ParseSeeds(input);

        Dictionary<MapType, List<long[]>> maps = new();
        AdvanceToLabel("seed-to-soil map:", sr);
        maps[MapType.SeedToSoil] = MapSection(sr);
        AdvanceToLabel("soil-to-fertilizer map:", sr);
        maps[MapType.SoilToFertilizer] = MapSection(sr);
        AdvanceToLabel("fertilizer-to-water map:", sr);
        maps[MapType.FertilizerToWater] = MapSection(sr);
        AdvanceToLabel("water-to-light map:", sr);
        maps[MapType.WaterToLight] = MapSection(sr);
        AdvanceToLabel("light-to-temperature map:", sr);
        maps[MapType.LightToTemperature] = MapSection(sr);
        AdvanceToLabel("temperature-to-humidity map:", sr);
        maps[MapType.TemperatureToHumidity] = MapSection(sr);
        AdvanceToLabel("humidity-to-location map:", sr);
        maps[MapType.HumidityToLocation] = MapSection(sr);

        return helper.Solve(maps);
        // read maps in sections
        // compute calculation necessary to move from map to map
        // (create helper methods that takes and input value and gives the destination directly,
        // to be used in the next map, and so on, to the location number)
        // return the smallest locaton number (day 1 solution)
    }

    public static long MapSeedToLocation(long seed, Dictionary<MapType, List<long[]>> maps)
    {
        var result = GetDestination(seed, maps[MapType.SeedToSoil]);
        result = GetDestination(result, maps[MapType.SoilToFertilizer]);
        result = GetDestination(result, maps[MapType.FertilizerToWater]);
        result = GetDestination(result, maps[MapType.WaterToLight]);
        result = GetDestination(result, maps[MapType.LightToTemperature]);
        result = GetDestination(result, maps[MapType.TemperatureToHumidity]);
        result = GetDestination(result, maps[MapType.HumidityToLocation]);
        return result;
    }

    private static long GetDestination(long i, List<long[]> referenceMap)
    {
        // find the map where i is. Assume only one match
        var map = referenceMap
            .SingleOrDefault(x => x[SourceRangeStart] <= i &&
                                  i <= x[SourceRangeStart] + x[RangeLength] - 1);

        if (map is null)
        {
            // Console.WriteLine("no map");
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

public enum MapType
{
    SeedToSoil,
    SoilToFertilizer,
    FertilizerToWater,
    WaterToLight,
    LightToTemperature,
    TemperatureToHumidity,
    HumidityToLocation
}