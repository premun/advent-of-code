using System.Numerics;

namespace _19;

record BeaconGroup
{
    public Vector3[] Beacons { get; }

    public float[][] RelativeDistances { get; }

    public BeaconGroup(Vector3[] beacons, float[][] relativeDistances)
    {
        Beacons = beacons;
        RelativeDistances = relativeDistances;
    }

    public BeaconGroup(Vector3[] beacons)
        : this(beacons, beacons
            .Select(coor => beacons
                .Where(c => c != coor)
                .Select(c => Vector3.DistanceSquared(c, coor))
                .ToArray())
            .ToArray())
    {
    }
}
