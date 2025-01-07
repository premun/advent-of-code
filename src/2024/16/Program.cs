using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines()
    .ParseAsArray();

var start = map.AllCoordinates().First(c => map.Get(c) == 'S');
var corner = new[]
{
    (Coor.Up, Coor.Left),
    (Coor.Up, Coor.Right),
    (Coor.Down, Coor.Left),
    (Coor.Down, Coor.Right),
};
var end = map.AllCoordinates().First(c => map.Get(c) == 'E');
var direction = Coor.Right;

// Places where we can change direction
var crossroads = map.AllCoordinates()
    .Where(c => map.Get(c) == '.' && corner.Any(d => map.Get(c + d.Item1) == '.' && map.Get(c + d.Item2) == '.'))
    .Append(start)
    .Append(end)
    .ToHashSet();

var neighbours = new Dictionary<Coor, List<(Coor Neighbour, int Distance)>>();

foreach (var c in crossroads)
{
    foreach (var dir in Coor.FourWayNeighbours)
    {
        var current = c;
        var dist = 0;
        do
        {
            current += dir;
            dist++;

            if (crossroads.Contains(current))
            {
                if (!neighbours.TryGetValue(c, out var list))
                {
                    list = [];
                    neighbours[c] = list;
                }
                list.Add((current, dist));
                break;
            }
        } while (map.Get(current) == '.');
    }
}

var scores = Resources.GetInputFileLines()
    .ParseAsArray(c => c switch
    {
        'S' => 0,
        '#' => int.MinValue,
        _ => int.MaxValue,
    });

var shortestPaths = new Dictionary<Coor, List<Coor>>();
var queue = new PriorityQueue<State, int>();
queue.Enqueue(new(start, Coor.Right, []), 0);

while (queue.TryDequeue(out var current, out var score))
{
    if (scores.Get(current.Position) < score) continue;

    scores.Set(current.Position, score);
    shortestPaths[current.Position] = current.Path;

    if (!neighbours.TryGetValue(current.Position, out var currentNeighbours))
    {
        continue;
    }

    foreach (var nc in currentNeighbours.Where(n => n.Neighbour != current.Position))
    {
        var newDirection = (current.Position - nc.Neighbour).Normalize();
        var newScore = score + nc.Distance + (newDirection == current.Direction ? 0 : 1000);
        if (scores.Get(nc.Neighbour) > newScore)
        {
            queue.Enqueue(new(nc.Neighbour, newDirection, [..current.Path, nc.Neighbour]), newScore);
        }
    }
}

var shortestPath = shortestPaths[end];
map.Print(c => shortestPath.Contains(c) ? ('*', ConsoleColor.Cyan) : (map.Get(c), ConsoleColor.White));

Console.WriteLine($"Part 1: {scores.Get(end)}");
Console.WriteLine($"Part 2: {""}");

file record struct State(Coor Position, Coor Direction, List<Coor> Path);
