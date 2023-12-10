using AdventOfCode.Common;

Console.WriteLine($"Part 1: {GetScore("23456789TJQKA", false)}");
Console.WriteLine($"Part 2: {GetScore("J23456789TQKA", true)}");

static int GetScore(string cardOrder, bool jokerEnabled) => Resources.GetInputFileLines()
    .Select(line => Hand.FromString(line, cardOrder, jokerEnabled))
    .Order()
    .Select((hand, rank) => (rank + 1) * hand.Bid)
    .Sum();

file record Hand : IComparable
{
    private readonly string _hand;
    private readonly string _cardOrder;

    public int Bid { get; }

    public HandType Type { get; }

    private Hand(string hand, int bid, string cardOrder, HandType type)
    {
        _hand = hand;
        _cardOrder = cardOrder;
        Bid = bid;
        Type = type;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return 0;
        }

        var hand = obj as Hand ?? throw new Exception();
        var t1 = (int)Type;
        var t2 = (int)hand.Type;
        var cmp = t1.CompareTo(t2);
        if (cmp != 0)
        {
            return cmp;
        }

        for (var i = 0; i < _hand.Length; ++i)
        {
            cmp = _cardOrder.IndexOf(_hand[i]).CompareTo(_cardOrder.IndexOf(hand._hand[i]));
            if (cmp != 0)
            {
                return cmp;
            }
        }

        return 0;
    }

    public static Hand FromString(string definition, string cardOrder, bool jokerEnabled)
    {
        if (definition.Split(' ') is not [string hand, string bid])
        {
            throw new ArgumentException("Invalid definition", nameof(definition));
        }

        return new Hand(hand, int.Parse(bid), cardOrder, GetType(hand, cardOrder, jokerEnabled));
    }

    private static HandType GetType(string hand, string cardOrder, bool jokerEnabled)
    {
        var histogram = new int[cardOrder.Length];
        int max = 0;
        for (int i = 0; i < hand.Length; i++)
        {
            var index = cardOrder.IndexOf(hand[i]);
            histogram[index] += 1;
            max = Math.Max(histogram[index], max);
        }

        return (max, jokerEnabled ? histogram[0] : 0) switch
        {
            (5, _) => HandType.FiveOfAkind,
            (4, 0) => HandType.FourOfaKind,
            (4, _) => HandType.FiveOfAkind,
            (3, 3) => histogram.Contains(2)
                    ? HandType.FiveOfAkind
                    : HandType.FourOfaKind,
            (3, 2) => HandType.FiveOfAkind,
            (3, 1) => HandType.FourOfaKind,
            (3, 0) => histogram.Contains(2)
                    ? HandType.FullHouse
                    : HandType.ThreeOfaKind,
            (2, 2) => histogram.Where(c => c == 2).Count() == 2
                    ? HandType.FourOfaKind
                    : HandType.ThreeOfaKind,
            (2, 1) => histogram.Where(c => c == 2).Count() == 2
                    ? HandType.FullHouse
                    : HandType.ThreeOfaKind,
            (2, 0) => histogram.Where(c => c == 2).Count() == 2
                    ? HandType.TwoPairs
                    : HandType.OnePair,
            (1, 1) => HandType.OnePair,
            (1, 0) => HandType.HighCard,
            _ => throw new InvalidOperationException(),
        };
    }
}

file enum HandType
{
    HighCard = 1,
    OnePair = 2,
    TwoPairs = 3,
    ThreeOfaKind = 4,
    FullHouse = 5,
    FourOfaKind = 6,
    FiveOfAkind = 7,
}
