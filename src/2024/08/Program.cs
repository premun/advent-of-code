using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines().ParseAsArray();

ILookup<char, Coor<int>> antennasByFrequency = map
    .AllCoordinates()
    .Where(c => map.Get(c) != '.')
    .ToLookup(c => map.Get(c));

Console.WriteLine($"Part 1: {GetAntinodeCount(false)}");
Console.WriteLine($"Part 2: {GetAntinodeCount(true)}");

int GetAntinodeCount(bool allInLine)
{
    return antennasByFrequency
        .SelectMany(group => group
            .AllCombinations(includeIdentities: false)
            .Where(pair => pair.Item1.Col < pair.Item2.Col) // We don't need both combinations (x,y) and (y,x)
            .SelectMany(pair => allInLine
                ? GetAntinodes2(pair.Item1, pair.Item2)
                : GetAntinodes1(pair.Item1, pair.Item2)))
        .Distinct()
        .Count();
}

IEnumerable<Coor> GetAntinodes1(Coor a1, Coor a2)
{
    var distance = a1 - a2;
    var antinode1 = a1 + distance;
    if (antinode1.InBoundsOf(map)) yield return antinode1;

    var antinode2 = a2 - distance;
    if (antinode2.InBoundsOf(map)) yield return antinode2;
}

IEnumerable<Coor> GetAntinodes2(Coor a1, Coor a2)
{
    var distance = a1 - a2;

    var antinode1 = a2 + distance;
    while (antinode1.InBoundsOf(map))
    {
        yield return antinode1;
        antinode1 = antinode1 + distance;
    }

    var antinode2 = a1 - distance;
    while (antinode2.InBoundsOf(map))
    {
        yield return antinode2;
        antinode2 = antinode2 - distance;
    }
}
