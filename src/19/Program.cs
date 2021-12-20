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

// Finds matching beacons from two scanners based on relative distances between them
static List<(int, int)> GetMatchingBeacons(BeaconGroup group1, BeaconGroup group2)
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
static (int, int, List<(int, int)>) GetMatchingGroups(List<BeaconGroup> groups)
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

var groups = scanners_.Select(x => new BeaconGroup(x.ToArray())).ToList();

while (groups.Count > 1)
{
    groups = groups.OrderBy(a => Guid.NewGuid()).ToList();

    var (i, j, matchingBeacons) = GetMatchingGroups(groups);

    var group1 = groups[i];
    var group2 = groups[j];

    Console.WriteLine($"From {groups.Count} joining {i} ({group1.Beacons.Length}) and {j} ({group2.Beacons.Length}) with {matchingBeacons.Count} matches");

    var selectedScanners = new List<(Vector3, float[])>();

    for (int x = 0; x < group1.Beacons.Length; x++)
    {
        if (matchingBeacons.Any(p => p.Item1 == x))
        {
            var matching = matchingBeacons.First(p => p.Item1 == x);
            selectedScanners.Add((
                group1.Beacons[x],
                group1.RelativeDistances[x].Concat(group2.RelativeDistances[matching.Item2]).ToArray()));
        }
        else
        {
            selectedScanners.Add((group1.Beacons[x], group1.RelativeDistances[x]));
        }
    }

    for (int x = 0; x < group2.Beacons.Length; x++)
    {
        if (!matchingBeacons.Any(p => p.Item2 == x))
        {
            selectedScanners.Add((group2.Beacons[x], group2.RelativeDistances[x]));
        }
    }

    var newGroup = new BeaconGroup(selectedScanners.Select(x => x.Item1).ToArray(), selectedScanners.Select(x => x.Item2).ToArray());

    Console.WriteLine($"     New group size: {newGroup.Beacons.Length}");

    groups.Remove(group1);
    groups.Remove(group2);
    groups.Add(newGroup);
}

Console.WriteLine($"Part 1: {groups.First().Beacons.Length}");
