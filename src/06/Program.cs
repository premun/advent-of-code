using Common;

var population = Resources.GetResourceFileLines("input.txt")
    .First()
    .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse)
    .ToArray();

static long[] CreateHistogram(IEnumerable<int> population)
{
    var histogram = new long[9];

    foreach (var i in population)
    {
        histogram[i]++;
    }

    return histogram;
}

static long Evolve(long[] histogram, int generations)
{
    for (int i = 0; i < generations; i++)
    {
        var readyToBreed = histogram[0];
        for (int j = 0; j < histogram.Length - 1; j++)
        {
            histogram[j] = histogram[j + 1];
        }

        histogram[8] = readyToBreed;
        histogram[6] += readyToBreed;
    }

    return histogram.Sum();
}

Console.WriteLine($"Part 1: {Evolve(CreateHistogram(population), 80)}");
Console.WriteLine($"Part 2: {Evolve(CreateHistogram(population), 256)}");
