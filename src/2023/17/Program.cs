using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines()
    .ParseAsArray(c => c - '0');

var queue = new PriorityQueue<List<Coor>, int>();
queue.Enqueue([Coor.Zero], 0);

var distances = new int[map.Height(), map.Width()]
    .InitializeWith(int.MaxValue);

while (queue.TryDequeue(out List<Coor<int>>? path, out var distance))
{
    var current = path.Last();

    if (distances.Get(current) > distance)
    {
        distances.Set(current, distance);

        //if (current == (map.Height() - 1, map.Width() - 1))
        //{
        //    Console.Clear();
        //    Resources.GetInputFileLines()
        //        .ParseAsArray()
        //        .Print(c => path.Contains(c) ? '.' : null);
        //}
        if (current == (map.Height() - 1, map.Width() - 1))
        {
            Console.WriteLine(distances.Get(current));
        }
    }

    foreach (var next in current.GetFourWayNeighbours())
    {
        if (!next.InBoundsOf(map))
        {
            continue;
        }

        var newDistance = distance + map.Get(next);

        // We already found a better path to here
        if (distances.Get(next) < newDistance)
            continue;

        // Three in a row
        if (path.Count > 3 &&
           (path[^4..^0].All(c => c.Row == next.Row)
            || path[^4..^0].All(c => c.Col == next.Col)))
            continue;

        // Do not reuse path
        if (path.Contains(next))
            continue;

        queue.Enqueue([.. path, next], newDistance);
    }
}

Console.WriteLine($"Part 1: {distances[map.Height() - 1, map.Width() - 1]}");
