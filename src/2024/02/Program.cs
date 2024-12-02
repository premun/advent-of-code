using AdventOfCode.Common;

var levels = Resources.GetInputFileLines("input.txt")
    .Select(line => line.SplitToNumbers(" "))
    .ToList();

var safeLevels = levels.Where(IsSafe);
var almostSafeLevels = levels.Where(IsAlmostSafe);

Console.WriteLine($"Part 1: {safeLevels.Count()}");
Console.WriteLine($"Part 2: {almostSafeLevels.Count()}");

static bool IsSafe(IList<int> level)
{
    var pairs = level.Take(level.Count - 1).Zip(level.Skip(1)).ToList();
    return pairs.Select(p => Math.Abs(p.First - p.Second)).All(p => p >= 1 && p <= 3)
        && (pairs.All(p => p.First > p.Second)
         || pairs.All(p => p.First < p.Second));
}

static bool IsAlmostSafe(IList<int> level)
{
    if (IsSafe(level))
    {
        return true;
    }

    for (int i = 0; i < level.Count; i++)
    {
        var newLevel = level.Take(i).Concat(level.Skip(i + 1)).ToList();
        if (IsSafe(newLevel))
        {
            return true;
        }
    }

    return false;
}
