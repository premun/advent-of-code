using AdventOfCode.Common;

var stones = Resources.GetInputFileLines()
    .First()
    .SplitToNumbers(" ")
    .Select(v => new Stone((ulong)v, 0));

Console.WriteLine(string.Join(", ", stones.Select(s => ReturnDifferentStones(s).Count)));

static Dictionary<ulong, int> ReturnDifferentStones(Stone startingStone)
{
    Dictionary<ulong, int> differentStones = new()
    {
        [startingStone.Number] = startingStone.Generation
    };
    var queue = new Queue<Stone>();
    queue.Enqueue(startingStone);

    while (queue.Count > 0)
    {
        if (!queue.TryDequeue(out Stone n)) continue;

        var (first, second) = Blink(n.Number);

        void TryEnqueue(Stone s)
        {
            if (differentStones.TryAdd(s.Number, s.Generation))
            {
                queue.Enqueue(s);
            }
        }

        TryEnqueue(new Stone(first, n.Generation + 1));

        if (second.HasValue)
        {
            TryEnqueue(new Stone(second.Value, n.Generation + 1));
        }
    }

    return differentStones;
}

static (ulong, ulong?) Blink(ulong number)
{
    if (number == 0)
    {
        return (1, null);
    }

    var digits = (int)(Math.Log10(number) + 1);
    if (digits % 2 != 0)
    {
        return (number * 2024, null);
    }

    var div = (ulong)Math.Pow(10, digits / 2);
    var left = number / div;
    var right = number - left * div;

    return (left, right);
}

readonly file record struct Stone(ulong Number, int Generation);
