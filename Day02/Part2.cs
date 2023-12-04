namespace Day02;

public class Part2
{
    public int Solve()
    {
        string? input;
        var powerSum = 0;

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

            // for each game, save the max value of each color.
            // multiply the max values together and sum them. that is the solution
            Dictionary<string, int> maxValues = new()
            {
                { "red", int.MinValue },
                { "green", int.MinValue },
                { "blue", int.MinValue },
            };
            
            foreach (var r in roundsArr)
            {
                Dictionary<string, int> round = new();
                var split = r.Split(',');
                
                foreach (var color in split)
                {
                    var splitColor = color.Trim().Split(" ");
                    round[splitColor[1]] = int.Parse(splitColor[0]);
                }

                foreach (var color in round)
                {
                    if (color.Value > maxValues[color.Key])
                    {
                        maxValues[color.Key] = color.Value;
                    }
                }
            }
            
            // multiply all values in maxvalues
            powerSum += maxValues.Values.Aggregate((total, next) => total * next);
        }

        return powerSum;
    }
}