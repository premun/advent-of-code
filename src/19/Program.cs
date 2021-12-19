using _19;
using Common;
using System.Numerics;

var lines = Resources.GetResourceFileLines("input.txt");

var scanners_ = new List<List<Vector3>>();

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

var scanners = scanners_.Select(x => x.ToArray()).ToArray();

// Finds relative distances between all beacons
static float[][] GetDistances(Vector3[] coors)
{
    var result = new List<float[]>();

    foreach (var coor in coors)
    {
        result.Add(coors
            .Where(c => c != coor)
            .Select(c => Vector3.DistanceSquared(c, coor))
            .ToArray());
    }

    return result.ToArray();
}

// Finds matching beacons from two scanners based on relative distances between them
static List<(int, int)> GetMatchingBeacons(Vector3[] group1, Vector3[] group2)
{
    var relativeDistances = new[] { group1, group2 }.Select(GetDistances).ToArray();

    var graph = new bool[relativeDistances[0].Length, relativeDistances[1].Length];

    for (int i = 0; i < relativeDistances[0].Length; i++)
    {
        for (int j = 0; j < relativeDistances[1].Length; j++)
        {
            graph[i, j] = relativeDistances[0][i].Intersect(relativeDistances[1][j]).Any();
        }
    }

    return BipartityMatcher.FindMaxBipartity(graph).ToList();
}

// Finds two scanners with at least 12 matching beacons
static (int, int, List<(int, int)>) GetMatchingGroups(List<Vector3[]> groups)
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

var groups = scanners.ToList();

while (groups.Count > 1)
{
    var (i, j, matchingBeacons) = GetMatchingGroups(groups);

    var group1 = groups[i];
    var group2 = groups[j];

    groups.Remove(group1);
    groups.Remove(group2);

    var newGroup = new List<Vector3>(group1);

    for (int x = 0; x < group2.Length; x++)
    {
        if (!matchingBeacons.Any(m => m.Item2 == x))
        {
            newGroup.Add(group2[x]);
        }
    }

    Console.WriteLine($"From {groups.Count} joining {i} and {j} with {matchingBeacons.Count} matches. New group size: {newGroup.Count}");
    groups.Add(newGroup.ToArray());
}

Console.WriteLine($"Part 1: {groups.First().Length}");
