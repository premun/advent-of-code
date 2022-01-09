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

var allCuboids = new List<(bool IsOn, Cuboid Cube)>();

foreach (var cuboid in cuboids)
{
    var count = allCuboids.Count;
    for (var i = 0; i < count; i++)
    {
        var otherCuboid = allCuboids[i];
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
