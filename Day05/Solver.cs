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