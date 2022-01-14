using System.Numerics;

namespace AdventOfCode._2021_19;

record Scanner
{
    public Vector3[] Beacons { get; }

    public float[][] RelativeDistances { get; }

    public Scanner(Vector3[] beacons, float[][] relativeDistances)
    {
        Beacons = beacons;
        RelativeDistances = relativeDistances;
    }

    public Scanner(Vector3[] beacons)
        : this(beacons, beacons
            .Select(coor => beacons
                .Where(c => c != coor)
                .Select(c => Vector3.DistanceSquared(c, coor))
                .ToArray())
            .ToArray())
    {
    }
}
