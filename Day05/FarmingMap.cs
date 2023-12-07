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

    /// <summary>
    /// Gets an ordered list of locations. The retuerned array containes the start and end value of
    /// the range, inclusive.
    /// </summary>
    /// <returns></returns>
    public List<long[]> GetOrderedLocations()
    {
        // DestinationRangeStart, destination plus len
        var map = _maps[MapType.HumidityToLocation];

        var locationRanges = map
            .Select(x => new long[]
            {
                x[DestinationRangeStart],
                x[DestinationRangeStart] + x[RangeLength] - 1
            }).OrderBy(x => x[0])
            .ToList();

        return locationRanges;
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

    public long MapLocationToSeed(long location)
    {
        var result = ReverseMap(location, _maps[MapType.HumidityToLocation]);
        result = ReverseMap(result, _maps[MapType.TemperatureToHumidity]);
        result = ReverseMap(result, _maps[MapType.LightToTemperature]);
        result = ReverseMap(result, _maps[MapType.WaterToLight]);
        result = ReverseMap(result, _maps[MapType.FertilizerToWater]);
        result = ReverseMap(result, _maps[MapType.SoilToFertilizer]);
        result = ReverseMap(result, _maps[MapType.SeedToSoil]);
        return result;
    }

    private static long GetDestination(long sourceValue, List<long[]> mapContainer)
    {
        // find the map where i is. Assume only one match
        var map = mapContainer.SingleOrDefault(x =>
        {
            var firstInRange = x[SourceRangeStart];
            var lastInRange = x[SourceRangeStart] + x[RangeLength] - 1;

            return firstInRange <= sourceValue && sourceValue <= lastInRange;
        });

        if (map is null)
        {
            return sourceValue;
        }

        var offset = sourceValue - map[SourceRangeStart];

        return map[DestinationRangeStart] + offset;
    }

    private static long ReverseMap(long destinationValue, List<long[]> mapContainer)
    {
        var map = mapContainer.SingleOrDefault(x =>
        {
            var firstInRange = x[DestinationRangeStart];
            var lastInRange = x[DestinationRangeStart] + x[RangeLength] - 1;

            return firstInRange <= destinationValue && destinationValue <= lastInRange;
        });

        if (map is null)
        {
            return destinationValue;
        }

        var offset = destinationValue - map[DestinationRangeStart];

        return map[SourceRangeStart] - offset;
    }
}