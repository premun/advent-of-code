using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines()
    .ParseAsArray(c => c - '0');

var starts = map.AllCoordinates()
    .Where(c => map.Get(c) == 0);

Console.WriteLine($"Part 1: {starts.Select(s => GetScore(s, false)).Sum()}");
Console.WriteLine($"Part 2: {starts.Select(s => GetScore(s, true)).Sum()}");

int GetScore(Coor position, bool distinctPaths)
{
    return distinctPaths
        ? GetReachableNines(position).Count
        : GetReachableNines(position).Distinct().Count();
}

List<Coor> GetReachableNines(Coor position, List<Coor>? reachableNines = null)
{
    reachableNines ??= [];
    var target = map.Get(position) + 1;
    if (target == 10)
    {
        reachableNines.Add(position);
        return reachableNines;
    }

    foreach (var n in position.GetFourWayNeighbours())
    {
        if (n.InBoundsOf(map) && map.Get(n) == target)
        {
            GetReachableNines(n, reachableNines);
        }
    }

    return reachableNines;
}
