using Common;

var depths = Resources.GetResourceFileLines("input.txt").Select(int.Parse).ToArray();

static int GetIncreaseCount(int[] depths, int lookback)
{
    return depths
        .Select((d, index) => (Current: d, Previous: index < lookback ? int.MaxValue : depths[index - lookback]))
        .Count(pair => pair.Current > pair.Previous);
}

Console.WriteLine($"Part 1: {GetIncreaseCount(depths, 1)}");
Console.WriteLine($"Part 2: {GetIncreaseCount(depths, 3)}");
