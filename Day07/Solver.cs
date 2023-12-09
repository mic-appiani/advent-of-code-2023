using System.Text.RegularExpressions;

public class Solver
{
    private int _solution = 0;
    private List<Hand> _handsRanking = new();

    public int Solve(int part)
    {
        using var sr = new StreamReader("input.txt");
        string? input;
        while (!sr.EndOfStream)
        {
            input = sr.ReadLine();

            var split = input.Split(' ');
            var hand = new Hand
            {
                Cards = split[0],
                Bet = int.Parse(split[1]),
                Score = Rank(split[0]),
            };

            _handsRanking.Add(hand);
        }

        _handsRanking.Sort((a, b) => a.Score.CompareTo(b.Score));

        for (int i = 0; i < _handsRanking.Count; i++)
        {
            var hand = _handsRanking[i];
            var winnings = hand.Bet * (i + 1);
            _solution += winnings;
        }

        return _solution;
    }

    private long Rank(string cards)
    {
        var score = 0;
        // calculate hand score
        score += ScoreForHandType(cards);

        // calculate and add card weight score
        score += ScoreForCardsOrder(cards);
        return score;
    }

    private int ScoreForCardsOrder(string cards)
    {
        var multiplier = 1;
        var score = 0;
        for (int i = cards.Length - 1; i >= 0; i--)
        {
            var value = CardValues[cards[i]];
            score += value * multiplier;
            multiplier *= 15;
        }

        return score;
    }

    private int ScoreForHandType(string cards)
    {
        var type = -1;
        // 7 possible types

        var dict = new Dictionary<char, byte>();
        for (int i = 0; i < cards.Length; i++)
        {
            var card = cards[i];
            if (!dict.TryAdd(card, 1))
            {
                dict[card]++;
            }
        }

        if (dict.Count == 5)
        {
            type = 1;
        }

        if (dict.Count == 4)
        {
            type = 2;
        }

        if (dict.Count == 3)
        {
            if (dict.Values.Any(x => x == 3))
            {
                type = 4;
            }
            else
            {
                type = 3;
            }
        }

        if (dict.Count == 2)
        {
            if (dict.Values.Any(x => x == 4))
            {
                type = 6;
            }
            else
            {
                type = 5;
            }
        }

        if (dict.Count == 1)
        {
            type = 7;
        }

        return type * 1_000_000;
    }

    private static readonly Dictionary<char, int> CardValues = new()
    {
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        { 'J', 11 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 },
    };
}

internal class Hand
{
    public string Cards { get; set; }
    public int Bet { get; set; }
    public long Score { get; set; }
}