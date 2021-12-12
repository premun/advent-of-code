using _12;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

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

    cave1.Paths.Add(cave2);
    cave2.Paths.Add(cave1);
}

static List<List<Cave>> FindAllPaths(Cave from, Cave to)
{
    return FindPaths(new(), from, to);
}

static List<List<Cave>> FindPaths(List<Cave> path, Cave current, Cave to)
{
    if (current == to)
    {
        return new() { path.Append(to).ToList() };
    }

    var paths = new List<List<Cave>>();

    foreach (var cave in current.Paths)
    {
        if (cave.IsSmall && path.Contains(cave))
        {
            continue;
        }

        paths.AddRange(FindPaths(path.Append(current).ToList(), cave, to));
    }

    return paths;
}

Console.WriteLine($"Part 1: {FindAllPaths(caves["start"], caves["end"]).Count}");
