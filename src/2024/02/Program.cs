using AdventOfCode.Common;

var levels = Resources.GetInputFileLines("input.txt")
    .Select(line => line.SplitToNumbers(" "))
    .ToList();

var safeLevels = levels.Where(IsSafe);
var almostSafeLevels = levels.Where(IsAlmostSafe);

Console.WriteLine($"Part 1: {safeLevels.Count()}");
Console.WriteLine($"Part 2: {almostSafeLevels.Count()}");

static bool IsSafe(IReadOnlyCollection<int> level)
{
    var differences = level
        .PairWithNext()
        .Select(pair => pair.Second - pair.First)
        .ToList();

    return differences.Select(Math.Abs).All(p => p >= 1 && p <= 3)
        && (differences.All(p => p > 0) || differences.All(p => p < 0));
}

static bool IsAlmostSafe(IReadOnlyCollection<int> level)
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
