using AdventOfCode._2021_12;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var caves = new Dictionary<string, Cave>();

foreach (var line in lines)
{
    var parts = line.Split('-');

    if (!caves.TryGetValue(parts[0], out var cave1))
    {
        cave1 = new Cave(parts[0]);
        caves.Add(parts[0], cave1);
    }

    if (!caves.TryGetValue(parts[1], out var cave2))
    {
        cave2 = new Cave(parts[1]);
        caves.Add(parts[1], cave2);
    }

    if (cave2.Name != "start" && cave1.Name != "end")
    {
        cave1.Paths.Add(cave2);
    }

    if (cave1.Name != "start" && cave2.Name != "end")
    {
        cave2.Paths.Add(cave1);
    }
}

static List<List<Cave>> FindAllPaths(Cave from, Cave to, bool allowSmallCaveReentry)
{
    return FindPaths(new(), from, to, allowSmallCaveReentry);
}

static List<List<Cave>> FindPaths(List<Cave> path, Cave current, Cave to, bool allowSmallCaveReentry)
{
    if (current == to)
    {
        return new() { path/*.Append(to).ToList()*/ };
    }

    path.Add(current);

    var paths = new List<List<Cave>>();

    var pathLeadsThroughTwoSmalls = path.Any(p => p.IsSmall && path.Count(q => q.Name == p.Name) == 2);

    foreach (var cave in current.Paths)
    {
        if (cave.IsSmall && (pathLeadsThroughTwoSmalls || !allowSmallCaveReentry) && path.Contains(cave))
        {
            continue;
        }

        paths.AddRange(FindPaths(path.ToList(), cave, to, allowSmallCaveReentry));
    }

    return paths;
}

Console.WriteLine($"Part 1: {FindAllPaths(caves["start"], caves["end"], false).Count}");
Console.WriteLine($"Part 2: {FindAllPaths(caves["start"], caves["end"], true).Count}");

//foreach (var path in FindAllPaths(caves["start"], caves["end"], true))
//{
//    Console.WriteLine(string.Join(",", path.Select(p => p.Name)));
//}
