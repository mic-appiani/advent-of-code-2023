using System.Text.RegularExpressions;

namespace Day04;

public class Solver
{
    private List<ScratchCard> _originalCards = new();
    private Stack<ScratchCard> _wonCards = new();
    private int _solution = 0;

    public int Solve(int part)
    {
        string? input;

        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            if (input is null)
            {
                throw new Exception("input should not be null");
            }

            string pattern = @"Card\s*([0-9]*):\s((?:[0-9]*\s*)*)\|\s*((?:[0-9]*\s*)*)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            var cardId = int.Parse(match.Groups[1].Value);

            var nums = match.Groups[2].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var winningNums =
                match.Groups[3].Value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            _originalCards.Add(new ScratchCard
            {
                Id = cardId,
                Numbers = nums,
                WinningNumbers = winningNums
            });

            var score = CalculateCardScore(nums, winningNums);

            if (score == 0)
            {
                continue;
            }

            if (part == 1)
            {
                _solution += (int)Math.Pow(2, score - 1);
            }
        }

        if (part == 2)
        {
            _solution += _originalCards.Count;

            // get n cards below the original card
            foreach (var card in _originalCards)
            {
                PullCardsIfWinner(card);
            }

            while (_wonCards.Count > 0)
            {
                _solution++;
                var card = _wonCards.Pop();
                PullCardsIfWinner(card);
            }
        }

        return _solution;
    }

    private void PullCardsIfWinner(ScratchCard card)
    {
        var score = CalculateCardScore(card.Numbers, card.WinningNumbers);

        if (score == 0)
        {
            return;
        }

        for (int i = card.Id; i < card.Id + score; i++)
        {
            _wonCards.Push(_originalCards[i] with{});
        }
    }

    private static int CalculateCardScore(string[] myNumsArray, string[] winningNumsArray)
    {
        var score = myNumsArray.Intersect(winningNumsArray).ToList();
        return score.Count;
    }
}

internal record ScratchCard
{
    public int Id { get; set; }
    public string[] Numbers { get; set; }
    public string[] WinningNumbers { get; set; }
}