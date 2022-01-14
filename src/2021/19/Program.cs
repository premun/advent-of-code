using System.Numerics;
using AdventOfCode._2021_19;
using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

var scanners_ = new List<List<Vector3>>();

var rotations = new Func<Vector3, Vector3>[]
{
    (Vector3 v) => new(v.X, v.Y, v.Z),
    (Vector3 v) => new(v.Y, v.Z, v.X),
    (Vector3 v) => new(-v.Y, v.X, v.Z),
    (Vector3 v) => new(-v.X, -v.Y, v.Z),

    (Vector3 v) => new(v.Y, -v.X, v.Z),
    (Vector3 v) => new(v.Z, v.Y, -v.X),
    (Vector3 v) => new(v.Z, v.X, v.Y),
    (Vector3 v) => new(v.Z, -v.Y, v.X),

    (Vector3 v) => new(v.Z, -v.X, -v.Y),
    (Vector3 v) => new(-v.X, v.Y, -v.Z),
    (Vector3 v) => new(v.Y, v.X, -v.Z),
    (Vector3 v) => new(v.X, -v.Y, -v.Z),

    (Vector3 v) => new(-v.Y, -v.X, -v.Z),
    (Vector3 v) => new(-v.Z, v.Y, v.X),
    (Vector3 v) => new(-v.Z, v.X, -v.Y),
    (Vector3 v) => new(-v.Z, -v.Y, -v.X),

    (Vector3 v) => new(-v.Z, -v.X, v.Y),
    (Vector3 v) => new(v.X, -v.Z, v.Y),
    (Vector3 v) => new(-v.Y, -v.Z, v.X),
    (Vector3 v) => new(-v.X, -v.Z, -v.Y),

    (Vector3 v) => new(v.Y, -v.Z, -v.X),
    (Vector3 v) => new(v.X, v.Z, -v.Y),
    (Vector3 v) => new(-v.Y, v.Z, -v.X),
    (Vector3 v) => new(-v.X, v.Z, v.Y),
};

foreach (var line in lines)
{
    if (line.StartsWith("--- scanner"))
    {
        scanners_.Add(new List<Vector3>());
    }
    else
    {
        var coor = line.Split(',').Select(int.Parse).ToArray();
        scanners_.Last().Add(new Vector3(coor[0], coor[1], coor[2]));
    }
}

// Finds matching beacons from two scanners based on relative distances between them
static List<(int, int)> GetMatchingBeacons(Scanner group1, Scanner group2)
{
    var graph = new bool[group1.RelativeDistances.Length, group2.RelativeDistances.Length];

    for (int i = 0; i < group1.RelativeDistances.Length; i++)
    {
        for (int j = 0; j < group2.RelativeDistances.Length; j++)
        {
            graph[i, j] = group1.RelativeDistances[i].Intersect(group2.RelativeDistances[j]).Count() > 10;
        }
    }

    return BipartityMatcher.FindMaxBipartity(graph).ToList();
}

// Finds two scanners with at least 12 matching beacons
static (int, int, List<(int, int)>) GetMatchingGroups(List<Scanner> groups)
{
    for (var i = 0; i < groups.Count; ++i)
    {
        for (var j = i + 1; j < groups.Count; ++j)
        {
            var matchingBeacons = GetMatchingBeacons(groups[i], groups[j]);

            if (matchingBeacons.Count >= 12)
            {
                return (i, j, matchingBeacons);
            }
        }
    }

    throw new Exception("No more matching groups");
}

int Part1(List<Scanner> scanners)
{
    while (scanners.Count > 1)
    {
        var (i, j, matchingBeacons) = GetMatchingGroups(scanners);

        var group1 = scanners[i];
        var group2 = scanners[j];

        //Console.WriteLine($"From {groups.Count} joining {i} ({group1.Beacons.Length}) and {j} ({group2.Beacons.Length}) with {matchingBeacons.Count} matches");

        var matchingBeacons1 = matchingBeacons.Select(m => group1.Beacons[m.Item1]).ToArray();
        var matchingBeacons2 = matchingBeacons.Select(m => group2.Beacons[m.Item2]).ToArray();

        var diff1 = matchingBeacons1[0] - matchingBeacons1[1];
        var diff2 = matchingBeacons2[0] - matchingBeacons2[1];

        var rotation = rotations.First(r => diff1 == r(diff2));

        var scannerDifference = matchingBeacons1[0] - rotation(matchingBeacons2[0]);

        var rotatedBeacons = group2.Beacons.Select(b => rotation(b) + scannerDifference);
        var newGroup = new Scanner(group1.Beacons.Union(rotatedBeacons).ToArray());

        //Console.WriteLine($"     New group size: {newGroup.Beacons.Length}");

        scanners.Remove(group1);
        scanners.Remove(group2);
        scanners.Add(newGroup);
    }

    return scanners.Single().Beacons.Length;
}

Console.WriteLine($"Part 1: {Part1(scanners_.Select(x => new Scanner(x.ToArray())).ToList())}");
