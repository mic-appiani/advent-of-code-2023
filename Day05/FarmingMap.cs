public class FarmingMap
{
    Dictionary<MapType, List<long[]>> _maps = new();
    private const int DestinationRangeStart = 0;
    private const int SourceRangeStart = 1;
    private const int RangeLength = 2;

    public void LoadAllMaps(StreamReader sr)
    {
        Solver.AdvanceToLabel("seed-to-soil map:", sr);
        _maps[MapType.SeedToSoil] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("soil-to-fertilizer map:", sr);
        _maps[MapType.SoilToFertilizer] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("fertilizer-to-water map:", sr);
        _maps[MapType.FertilizerToWater] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("water-to-light map:", sr);
        _maps[MapType.WaterToLight] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("light-to-temperature map:", sr);
        _maps[MapType.LightToTemperature] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("temperature-to-humidity map:", sr);
        _maps[MapType.TemperatureToHumidity] = Solver.MapSection(sr);
        Solver.AdvanceToLabel("humidity-to-location map:", sr);
        _maps[MapType.HumidityToLocation] = Solver.MapSection(sr);
    }

    public long MapSeedToLocation(long seed)
    {
        var result = GetDestination(seed, _maps[MapType.SeedToSoil]);
        result = GetDestination(result, _maps[MapType.SoilToFertilizer]);
        result = GetDestination(result, _maps[MapType.FertilizerToWater]);
        result = GetDestination(result, _maps[MapType.WaterToLight]);
        result = GetDestination(result, _maps[MapType.LightToTemperature]);
        result = GetDestination(result, _maps[MapType.TemperatureToHumidity]);
        result = GetDestination(result, _maps[MapType.HumidityToLocation]);
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
}