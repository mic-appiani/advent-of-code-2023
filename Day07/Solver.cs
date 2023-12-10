using System.Reflection;
using System.Runtime.CompilerServices;

namespace Day07;

public class Solver
{
    private int _solution;
    private readonly List<Hand> _handsRanking = new();

    public int Solve(int part)
    {
        using var sr = new StreamReader("input.txt");
        while (!sr.EndOfStream)
        {
            var input = sr.ReadLine();
            var split = input!.Split(' ');
            var hand = new Hand
            {
                Cards = split[0],
                Bet = int.Parse(split[1]),
                Score = Rank(split[0], part),
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

    private long Rank(string cards, int part)
    {
        var score = 0;
        // calculate hand score
        score += ScoreForHandType(cards, part);

        // calculate and add card weight score
        score += ScoreForCardsOrder(cards, part);
        return score;
    }

    private int ScoreForCardsOrder(string cards, int part)
    {
        var multiplier = 1;
        var score = 0;
        var deck = part == 1 ? CardValues : CardValuesWithJoker;

        for (int i = cards.Length - 1; i >= 0; i--)
        {
            var value = deck[cards[i]];
            score += value * multiplier;
            multiplier *= 15;
        }

        return score;
    }

    private int ScoreForHandType(string cards, int part)
    {
        var cardArray = cards.ToCharArray();

        var type = HandType.Unknown;
        // 7 possible types

        var dict = new Dictionary<char, CardData>();
        for (byte i = 0; i < cardArray.Length; i++)
        {
            var card = cardArray[i];
            if (dict.TryGetValue(card, out var value))
            {
                value.Frequency++;
                value.Positions.Add(i);
            }
            else
            {
                dict.Add(card, new CardData
                {
                    Frequency = 1,
                    Positions = [i]
                });
            }
        }

        if (part == 2)
        {
            SubstituteJokers(cardArray, dict);
        }

        if (dict.Count == 5)
        {
            type = HandType.HighCard;
        }

        if (dict.Count == 4)
        {
            type = HandType.Pair;
        }

        if (dict.Count == 3)
        {
            type = dict.Values.Any(x => x.Frequency == 3)
                ? HandType.ThreeOfAKind
                : HandType.TwoPairs;
        }

        if (dict.Count == 2)
        {
            type = dict.Values.Any(x => x.Frequency == 4)
                ? HandType.FourOfAKind
                : HandType.FullHouse;
        }

        if (dict.Count == 1)
        {
            type = HandType.FiveOfAKind;
        }

        return (int)type * 1_000_000;
    }

    private void SubstituteJokers(char[] cardArray, Dictionary<char, CardData> dict)
    {
        if (!dict.TryGetValue('J', out CardData? joker))
        {
            return;
        }

        char mostFrequentCard = dict
            .OrderByDescending(x => x.Value.Positions.Count)
            .FirstOrDefault(x => x.Key != 'J').Key;

        if (mostFrequentCard == default)
        {
            mostFrequentCard = 'A';
            dict.Clear();
            dict['A'] = new CardData { Frequency = 5, Positions = [0, 1, 2, 3, 4] };
        }

        for (byte i = 0; i < joker.Positions.Count; i++)
        {
            cardArray[joker.Positions[i]] = mostFrequentCard;
            
            dict[mostFrequentCard].Frequency++;
            dict[mostFrequentCard].Positions.Add(i);
            
            
        }

        dict.Remove('J');
    }


    private static readonly Dictionary<char, int> CardValuesWithJoker = new()
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
        { 'J', 1 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 },
    };

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

internal class CardData
{
    public byte Frequency { get; set; }
    public List<byte> Positions { get; set; }
}

internal class Hand
{
    public string Cards { get; set; } = string.Empty;
    public int Bet { get; set; }
    public long Score { get; set; }
}

public enum HandType
{
    Unknown = 0,
    HighCard = 1,
    Pair = 2,
    TwoPairs = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7,
}