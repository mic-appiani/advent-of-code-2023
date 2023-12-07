namespace Day05;

public interface IDay5Solver
{
    void ParseSeeds(string input);
    long Solve(Dictionary<MapType, List<long[]>> maps);
}