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

/*static long GetNumberOfDistinctPoints(IList<Cuboid> cuboids)
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
}*/

var allCuboids = new List<(bool IsOn, Cuboid Cube)>();

foreach (var cuboid in cuboids)
{
    var count = allCuboids.Count;
    for (var i = 0; i < count; i++)
    {
        (bool IsOn, Cuboid Cube) otherCuboid = allCuboids[i];
        if (cuboid.Cube.TryIntersect(otherCuboid.Cube, out Cuboid? intersection))
        {
            allCuboids.Add((!otherCuboid.IsOn, intersection));
        }
    }

    if (cuboid.IsOn)
    {
        allCuboids.Add(cuboid);
    }
}

ulong initializationVolume = 0;
ulong volume = 0;

var limitation = new Cuboid(new Dim(-50, 50), new Dim(-50, 50), new Dim(-50, 50));

foreach (var cuboid in allCuboids)
{
    if (cuboid.Cube.TryIntersect(limitation, out Cuboid? intersection))
    {
        if (cuboid.IsOn)
        {
            initializationVolume += intersection.Volume;
        }
        else
        {
            initializationVolume -= intersection.Volume;
        }
    }

    if (cuboid.IsOn)
    {
        volume += cuboid.Cube.Volume;
    }
    else
    {
        volume -= cuboid.Cube.Volume;
    }
}

Console.WriteLine($"Part 1: {initializationVolume}");
Console.WriteLine($"Part 2: {volume}");

/*var points = new bool[101, 101, 101];
volume = 0;

foreach (var c in cuboids)
{
    Console.WriteLine($"{c.Cube} ({c.Cube.Volume})");

    for (int x = Math.Max(c.Cube.X.Min, -50); x <= Math.Min(c.Cube.X.Max, 50); x++)
    {
        for (int y = Math.Max(c.Cube.Y.Min, -50); y <= Math.Min(c.Cube.Y.Max, 50); y++)
        {
            for (int z = Math.Max(c.Cube.Z.Min, -50); z <= Math.Min(c.Cube.Z.Max, 50); z++)
            {
                var prev = points[x + 50, y + 50, z + 50];

                if (prev != c.IsOn)
                {
                    if (prev)
                    {
                        volume--;
                    }
                    else
                    {
                        volume++;
                    }

                    points[x + 50, y + 50, z + 50] = c.IsOn;
                }
            }
        }
    }
}

Console.WriteLine($"Part 1: {volume}");*/
