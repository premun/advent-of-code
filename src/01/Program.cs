using Common;

var lines = Resources.GetResourceFile("input.txt").Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var lastDepth = int.MaxValue;
var increases = 0;

// Part 1
foreach (var depth in lines.Select(int.Parse))
{
    if (depth > lastDepth)
    {
        increases++;
    }

    lastDepth = depth;
}

Console.WriteLine($"Part 1: {increases}");

// Part 2
increases = 0;
lastDepth = int.MaxValue;

var allDepths = lines.Select(int.Parse).ToArray();

for (int i = 3; i < allDepths.Length; i++)
{
    if (allDepths[i - 3] < allDepths[i])
    {
        increases++;
    }
}

Console.WriteLine($"Part 2: {increases}");
