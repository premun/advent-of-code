using System.Collections.Concurrent;
using AdventOfCode.Common;

var stones = Resources.GetInputFileLines()
    .First()
    .SplitToNumbers(" ")
    .Select(v => new Stone((ulong)v, 0))
    .ToArray();

var differentStones = new HashSet<ulong>();
foreach (var stone in stones)
{
    foreach (var s in GetDifferentStones(stone).Keys.Concat(stones.Select(s => s.Number)))
    {
        differentStones.Add(s);
    }
}

Console.WriteLine($"Found {differentStones.Count} different stones");
const int step = 25;

var stoneExpansionMap = new ConcurrentDictionary<ulong, ConcurrentDictionary<ulong, ulong>>();
Parallel.ForEach(differentStones, stone =>
{
    stoneExpansionMap[stone] = ExpandStones([new Stone(stone, 0)], step, []);
});

Console.WriteLine("Stone expansion map finished");

var stonesAfterStep = ExpandStones(stones, step, []);

Console.WriteLine($"Stones after  {step}: {stonesAfterStep.Values.Sum()}");

for (int i = step; i < step * 4;)
{
    var stonesAfter = new ConcurrentDictionary<ulong, ulong>();
    foreach (var stone in stonesAfterStep)
    {
        foreach (var newStone in stoneExpansionMap[stone.Key])
        {
            var newStoneCount = newStone.Value * stone.Value;
            stonesAfter.AddOrUpdate(newStone.Key, newStoneCount, (_, previous) => previous + newStoneCount);
        }
    }

    i += step;
    Console.WriteLine($"Stones after {i}: {stonesAfter.Values.Sum()}");
    stonesAfterStep = stonesAfter;
}

static ConcurrentDictionary<ulong, ulong> ExpandStones(IEnumerable<Stone> stones, int blinkTimes, ConcurrentDictionary<ulong, ulong> expandedStones)
{
    var queue = new ConcurrentQueue<Stone>();

    foreach (var s in stones)
    {
        queue.Enqueue(s);
        while (!queue.IsEmpty)
        {
            Parallel.ForEach(queue, _ =>
            {
                if (!queue.TryDequeue(out Stone stone)) return;

                if (stone.Generation == blinkTimes)
                {
                    expandedStones.AddOrUpdate(stone.Number, 1, (_, value) => value + 1);
                    return;
                }

                var newGeneration = stone.Generation + 1;
                var newStones = Blink(stone.Number);

                queue.Enqueue(new Stone(newStones.Item1, newGeneration));

                if (newStones.Item2.HasValue)
                {
                    queue.Enqueue(new Stone(newStones.Item2.Value, newGeneration));
                }
            });
        }
    }

    return expandedStones;
}

static Dictionary<ulong, int> GetDifferentStones(Stone startingStone)
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
        var ng = n.Generation + 1;

        if (differentStones.TryAdd(first, ng))
        {
            queue.Enqueue(new Stone(first, ng));
        }

        if (second.HasValue)
        {
            if (differentStones.TryAdd(second.Value, ng))
            {
                queue.Enqueue(new Stone(second.Value, ng));
            }
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
