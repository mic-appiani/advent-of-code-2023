public class Solver
{
    private int _solution;

    public int Solve(int part)
    {
        using var sr = new StreamReader("input.txt");

        while (!sr.EndOfStream)
        {
            var input = sr.ReadLine();

            var sequences = new List<List<int>>();
            var sequence = input!.Split(' ')
                .Select(x => int.Parse(x)).ToList();

            var allZeros = false;
            while (!allZeros)
            {
                sequences.Add(sequence);
                var newSequence = new List<int>();

                var zeros = 0;
                for (int i = 1; i < sequence.Count; i++)
                {
                    var diff = sequence[i] - sequence[i - 1];

                    if (diff == 0)
                    {
                        zeros++;
                    }

                    newSequence.Add(diff);
                }

                if (zeros == newSequence.Count)
                {
                    allZeros = true;
                }

                sequence = newSequence;
            }

            if (part == 1)
            {
                _solution += sequences.Select(x => x.Last()).Sum();
            }
            else if (part == 2)
            {
                var current = sequences.Last()[0];
                
                for (int i = sequences.Count - 2; i >= 0; i--)
                {
                    current = sequences[i][0] - current;
                }
                
                _solution += current;
            }
        }

        return _solution;
    }
}