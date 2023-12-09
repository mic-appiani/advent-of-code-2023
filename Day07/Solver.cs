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

        // todo: find a way to rank each hand to solve both a tie breaker (sum card value multiplied
        // by position, left side most significant), and the general rank based on hand composition
        // (the latter having more weight).

        // lower value hands has smaller score, lowest rank is 1 (add 1 to index)

        return _solution;
    }

    private long Rank(string cards)
    {
        var score = 0;
        // calculate hand score
        score += ScoreForHandType(cards);
        
        // calculate and add card weight score
        score = ScoreForCardsOrder(cards);
        return score;
    }

    private int ScoreForCardsOrder(string cards)
    {
        // iterate string right to left
        // get the value of the card for each char
        // multiply it by 10 for each position after the first.
        // return the sum
        return 0;
    }

    private int ScoreForHandType(string cards)
    {
        return 0;
    }
}

internal class Hand
{
    public string Cards { get; set; }
    public int Bet { get; set; }
    public long Score { get; set; }
}