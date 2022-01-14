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

(Scanner MergedBeacons, List<Vector3> ScannerPositions) MergeScanners(List<Scanner> scanners)
{
    var mainGroup = scanners[0];
    scanners.RemoveAt(0);

    var scannerPositions = new List<Vector3>
    {
        Vector3.Zero
    };

    // We could also find matching groups more efficiently
    // but then it would be painful to get scanner positions
    while (scanners.Count > 0)
    {
        foreach (var group in scanners)
        {
            var matchingBeacons = GetMatchingBeacons(mainGroup, group);
            if (matchingBeacons.Count < 12)
            {
                continue;
            }

            var matchingBeacons1 = matchingBeacons.Select(m => mainGroup.Beacons[m.Item1]).ToArray();
            var matchingBeacons2 = matchingBeacons.Select(m => group.Beacons[m.Item2]).ToArray();

            var diff1 = matchingBeacons1[0] - matchingBeacons1[1];
            var diff2 = matchingBeacons2[0] - matchingBeacons2[1];

            var rotation = rotations.First(r => diff1 == r(diff2));
            var scannerDifference = matchingBeacons1[0] - rotation(matchingBeacons2[0]);
            var rotatedBeacons = group.Beacons.Select(b => rotation(b) + scannerDifference);

            mainGroup = new Scanner(mainGroup.Beacons.Union(rotatedBeacons).ToArray());
            scannerPositions.Add(scannerDifference);
            scanners.Remove(group);

            break;
        }
    }

    return (mainGroup, scannerPositions);
}

var mergedScanners = MergeScanners(scanners_.Select(x => new Scanner(x.ToArray())).ToList());
var distances =
    from v1 in mergedScanners.ScannerPositions
    from v2 in mergedScanners.ScannerPositions
    where v1 != v2
    select Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y) + Math.Abs(v1.Z - v2.Z);

Console.WriteLine($"Part 1: {mergedScanners.MergedBeacons.Beacons.Length}");
Console.WriteLine($"Part 2: {distances.Max()}");
