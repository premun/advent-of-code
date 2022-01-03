using _22;
using Common;

var cuboids = Resources.GetResourceFileLines("input.txt")
    .Select(line => line.Split(" "))
    .Select(parts =>
    (   parts[0] == "on",
        parts[1]
            .Split(",")
            .Select(d => d.Substring(2))
            .Select(d => d.Split("..").Select(int.Parse).ToArray()).ToArray()
    ))
    .Select(parts =>
    (
        IsOn: parts.Item1,
        Cube: new Cuboid(
            new(parts.Item2[0][0], parts.Item2[0][1]),
            new(parts.Item2[1][0], parts.Item2[1][1]),
            new(parts.Item2[2][0], parts.Item2[2][1]))
    ))
    .ToList();

static long GetNumberOfDistinctPoints(IList<Cuboid> cuboids)
{
    if (cuboids.Count == 0)
    {
        return 0;
    }

    if (cuboids.Count == 1)
    {
        return cuboids.First().Volume;
    }

    var intersections = new List<Cuboid>();

    for (int i = 0; i < cuboids.Count; i++)
    {
        var current = cuboids[i];

        for (int j = 0; j < i; j++)
        {
            var other = cuboids[j];

            if (current.TryIntersect(other, out var intersection))
            {
                intersections.Add(intersection);
            }
        }
    }

    return cuboids.Select(c => c.Volume).Sum() - GetNumberOfDistinctPoints(intersections);
}

var buffer = 

for (int i = 0; i < cuboids.Count; i++)
{
    var current = cuboids[i];

    
}


/*int count = cuboids.Count;
long onCount = 0;
for (int i = 0; i < count; i++)
{
    var current = cuboids[i];

    Console.WriteLine($"{current} ({current.Cube.Volume}):");

    var intersections = new List<Cuboid>();

    int prevCount = i;
    for (int j = 0; j < prevCount; j++)
    {
        var prev = cuboids[j];

        if (prev.Cube.TryIntersect(current.Cube, out var intersection))
        {
            intersections.Add(intersection);

            // cubes.Insert(i, (current.IsOn, intersection, current.Depth + prev.Depth));

            Console.WriteLine($"   {intersection}  {intersection.Volume}");

            //i++;

            if (prev.IsOn)
            {
                onCount -= intersection.Volume;
            }
        }

        Console.WriteLine($"   {onCount}");
    }
}*/

var points = new Dictionary<(int, int, int), bool>();

foreach (var c in cuboids)
{
    Console.WriteLine($"{c.Cube} ({c.Cube.Volume})");

    var coors =
        from x in c.Cube.X.Points
        from y in c.Cube.Y.Points
        from z in c.Cube.Z.Points
        select (x, y, z);

    foreach (var coor in coors)
    {
        if (c.IsOn)
        {
            points[coor] = true;
        }
        else if (points.ContainsKey(coor))
        {
            points.Remove(coor);
        }
    }
}

Console.WriteLine($"Part 1: {points.Count(p => p.Value)}");
