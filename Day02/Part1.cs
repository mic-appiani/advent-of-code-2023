namespace Day02;

public class Part1
{
    public static Dictionary<string, int> maxConditions = new()
    {
        { "red", 12 },
        { "green", 13 },
        { "blue", 14 },
    };

    public int Solve()
    {
        string? input;
        var idSum = 0;

        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            if (input is null)
            {
                break;
            }

            var game = input.Split(':');
            var roundsArr = game[1].Split(';');

            var gameIsPossible = true;
            foreach (var r in roundsArr)
            {
                Dictionary<string, int> round = new();
                var split = r.Split(',');
                foreach (var color in split)
                {
                    var splitColor = color.Trim().Split(" ");
                    round[splitColor[1]] = int.Parse(splitColor[0]);
                }

                if (!PossibleRound(round))
                {
                    gameIsPossible = false;
                    break;
                }
            }

            if (gameIsPossible)
            {
                // add id number to sum
                var id = game[0].Trim().Split(' ')[1];

                idSum += int.Parse(id);
            }
        }

        return idSum;
    }

    private bool PossibleRound(Dictionary<string, int> round)
    {
        foreach (var color in round)
        {
            if ((maxConditions.TryGetValue(color.Key, out var maxValue) &&
                 color.Value <= maxValue) == false)
            {
                return false;
            }
        }

        return true;
    }
}