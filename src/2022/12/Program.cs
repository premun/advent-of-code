using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

char[,] map = Resources.GetInputFileLines().ParseAsArray(c => c);

Coor Find(char c) =>
   (from row in Enumerable.Range(0, map.GetLength(0))
    from col in Enumerable.Range(0, map.GetLength(1))
    where map[row, col] == c
    select new Coor(row, col)).First();

var S = Find('S');
var E = Find('E');

map.Set(S, 'a');
map.Set(E, 'z');

static int GetShortestDistance(char[,] map, Coor start, Func<Coor, Coor, bool> CanTravel, Func<Coor, bool> IsEnd)
{
    int[,] distances = Resources.GetInputFileLines().ParseAsArray(_ => int.MaxValue);
    distances.Set(start, 0);

    var queue = new PriorityQueue<Coor, int>();
    queue.Enqueue(start, 0);

    while (queue.TryDequeue(out Coor? current, out int distance))
    {
        if (IsEnd(current)) return distance;

        var neighbours = Coor.FourWayNeighbours
            .Select(n => n + current)
            .Where(n => n.InBoundsOf(map) && CanTravel(current, n));

        foreach (var neighbour in neighbours)
        {
            var newDistance = distance + 1;
            if (newDistance < distances.Get(neighbour))
            {
                distances.Set(neighbour, newDistance);
                queue.Enqueue(neighbour, newDistance);
            }
        }
    }

    throw new Exception("Path not found");
}

Console.WriteLine($"Part 1: {GetShortestDistance(map, S, (c, n) => map.Get(n) - map.Get(c) <= 1, c => c == E)}");
Console.WriteLine($"Part 2: {GetShortestDistance(map, E, (c, n) => map.Get(c) - map.Get(n) <= 1, c => map.Get(c) == 'a')}");
