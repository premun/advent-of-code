using System.Collections.Concurrent;
using AdventOfCode.Common;

var stones = Resources.GetInputFileLines()
    .First()
    .SplitToNumbers(" ")
    .Select(v => new Stone((ulong)v, 0));

Console.WriteLine(string.Join(", ", stones.Select(s => ReturnDifferentStones(s.Number).Count)));

static HashSet<ulong> ReturnDifferentStones(ulong number)
{
    HashSet<ulong> differentStones = [ number ];
    var queue = new ConcurrentQueue<ulong>();
    queue.Enqueue(number);
    var previousCount = 0;

    while (!queue.IsEmpty && previousCount != differentStones.Count)
    {
        previousCount = differentStones.Count;

        if (!queue.TryDequeue(out ulong n)) continue;

        var (first, second) = Blink(n);

        queue.Enqueue(first);
        differentStones.Add(first);

        if (second.HasValue)
        {
            queue.Enqueue(second.Value);
            differentStones.Add(second.Value);
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

/*var queue = new ConcurrentQueue<Stone>();

var maxGenerations = 75;
var total = 0UL;

var differentStones = new ConcurrentDictionary<ulong, bool>();

foreach (var s in stones)
{
    Console.WriteLine("Starting stone " + s.Number);
    queue.Enqueue(s);
    while (!queue.IsEmpty)
    {
        Console.Clear();
        Console.Write("[");
        var gen = queue.First().Generation;
        Console.Write(new string('|', gen));
        Console.Write(new string(' ', maxGenerations - gen));
        Console.WriteLine($"] {gen} / {maxGenerations} ({queue.Count})");
        Console.WriteLine("Total: " + total);
        Console.WriteLine("Different ones: " + differentStones.Keys.Count);
        //Console.WriteLine(string.Join(", ", queue.Select(s => $"{s.Number},{s.Generation}")));

        Parallel.ForEach(queue, _ =>
        {
            if (!queue.TryDequeue(out Stone stone)) return;
            var newGeneration = stone.Generation + 1;

            if (stone.Number == 0)
            {
                Enqueue(new Stone(1, newGeneration));
                return;
            }

            var digits = (int)(Math.Log10(stone.Number) + 1);
            if (digits % 2 != 0)
            {
                Enqueue(new Stone(stone.Number * 2024, newGeneration));
                return;
            }

            var div = (ulong)Math.Pow(10, digits / 2);
            var left = stone.Number / div;
            var right = stone.Number - left * div;

            Enqueue(new Stone(left, newGeneration));
            Enqueue(new Stone(right, newGeneration));
        });
    }
}

Console.WriteLine(total);

void Enqueue(Stone stone)
{
    if (stone.Generation == maxGenerations)
    {
        lock (stones)
        {
            //Console.WriteLine(stone.Number);
            Interlocked.Increment(ref total);
        }
    }
    else
    {
        queue.Enqueue(stone);
        differentStones.TryAdd(stone.Number, true);
    }
}

Console.WriteLine($"Part 1: {total}");
Console.WriteLine($"Part 2: {""}");*/

readonly file record struct Stone(ulong Number, int Generation);
