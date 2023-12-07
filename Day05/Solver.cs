using System.Diagnostics;
using System.Text.RegularExpressions;
using Day05;

public class Solver
{
    public long Solve(IDay5Solver helper)
    {
        string? input;

        using var sr = new StreamReader("input.txt");
        input = sr.ReadLine();

        helper.ParseSeeds(input);

        FarmingMap farmingMap = new();
        farmingMap.LoadAllMaps(sr);

        return helper.Solve(farmingMap);
        // read maps in sections
        // compute calculation necessary to move from map to map
        // (create helper methods that takes and input value and gives the destination directly,
        // to be used in the next map, and so on, to the location number)
        // return the smallest locaton number (day 1 solution)
    }


    public static List<long[]> MapSection(StreamReader sr)
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

    public static void AdvanceToLabel(string label, StreamReader sr)
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