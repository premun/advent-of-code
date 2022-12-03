using AdventOfCode.Common;

var positions = Resources.GetInputFileLines().First().SplitToNumbers();

static (int[], int, int) CreateHistogram(IEnumerable<int> positions)
{
    var min = positions.Min();
    var max = positions.Max();

    var histogram = new int[max - min + 1];

    foreach (var i in positions)
    {
        histogram[i - min]++;
    }

    return (histogram, min, max);
}

static long GetFuelNeeded(int[] histogram, int position, bool constantConsumption)
{
    long fuel = 0;

    for (int i = 0; i < histogram.Length; i++)
    {
        long distance = Math.Abs(i - position);

        if (!constantConsumption)
        {
            distance = distance * (distance + 1) / 2;
        }

        fuel += distance * histogram[i];
    }

    return fuel;
}

static long GetMinFuelNeeded(IEnumerable<int> positions, bool constantConsumption)
{
    var (histogram, min, max) = CreateHistogram(positions);

    return Enumerable.Range(min, max - min).Select(i => GetFuelNeeded(histogram, i - min, constantConsumption)).Min();
}

Console.WriteLine($"Part 1: {GetMinFuelNeeded(positions, true)}");
Console.WriteLine($"Part 2: {GetMinFuelNeeded(positions, false)}");
