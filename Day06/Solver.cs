using System.Text.RegularExpressions;

public class Solver
{
    private int[] _times = default!;
    private long[] _distances = default!;

    public int Solve(int part)
    {
        if (part == 1)
        {
            string? input;

            using var sr = new StreamReader("input.txt");

            input = sr.ReadLine();

            var pattern = @"([0-9]+)";
            var regex = new Regex(pattern);
            _times = regex.Matches(input!).Select(x => int.Parse(x.Value)).ToArray();

            input = sr.ReadLine();
            _distances = regex.Matches(input!).Select(x => long.Parse(x.Value)).ToArray();
        }
        else if (part == 2)
        {
            _times = new int[] { 53897698 };
            _distances = new long[] { 313109012141201 };
        }

        var wins = new int[_distances.Length];
        for (int i = 0; i < _times.Length; i++)
        {
            var distanceToBeat = _distances[i];

            var remainingTime = _times[i];
            long speed = 0;
            while (remainingTime > 0)
            {
                long distance = speed * remainingTime;

                if (distance > distanceToBeat)
                {
                    wins[i]++;
                }

                speed++;
                remainingTime--;
            }
        }

        return wins.Aggregate((curr, next) => curr * next);
    }
}