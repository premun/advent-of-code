using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2023_23;

internal class LongestPathResolverWithSlopes(string[] map)
    : LongestPathResolver(map.ParseAsArray())
{
    public override int FindLongestPath()
    {
        Dictionary<Junction, int> distances = Junctions
            .ToDictionary(c => c.Value, c => 0);

        PriorityQueue<List<Junction>, int> queue = new(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        queue.Enqueue([Junctions[Start]], 0);

        while (queue.TryDequeue(out var path, out int distance))
        {
            var junction = path.Last();
            if (distances[junction] > distance)
            {
                continue;
            }

            foreach (var neighbour in junction.Neighbours)
            {
                if (path.Contains(neighbour.Junction))
                {
                    continue;
                }

                var newDistance = distance + neighbour.Distance;
                if (distances[neighbour.Junction] > newDistance)
                {
                    continue;
                }

                distances[neighbour.Junction] = newDistance;
                queue.Enqueue([.. path, neighbour.Junction], newDistance);
            }
        }

        return distances[Junctions[End]];
    }

    protected override void ConnectAllJunctions()
        => ConnectJunctions(Junctions[Start], Start, Coor.Down, 0, true);
}

internal class LongestPathResolverWithoutSlopes(string[] map)
    : LongestPathResolver(map.ParseAsArray(c => c == '<' || c == '>' || c == '^' || c == 'v' ? '.' : c))
{
    public override int FindLongestPath()
    {
        return FindLongestPathRecursive(Junctions[Start], Junctions[End], [Junctions[Start]], 0);
    }

    // :(((( Brute force :(((
    // Need to find a longest path algorithm for cyclic directed graphs
    private static int FindLongestPathRecursive(Junction start, Junction end, List<Junction> currentPath, int distance)
    {
        var current = currentPath.Last();
        if (current == end)
        {
            return distance;
        }

        var result = 0;

        foreach (var n in current.Neighbours)
        {
            if (currentPath.Contains(n.Junction))
            {
                continue;
            }

            var longest = FindLongestPathRecursive(start, end, [.. currentPath, n.Junction], distance + n.Distance);
            result = Math.Max(result, longest);
        }

        return result;
    }

    protected override void ConnectAllJunctions()
    {
        foreach (var junction in Junctions.Values)
        {
            foreach (var direction in GetPossibleDirections(junction.Position, Coor.Zero))
            {
                ConnectJunctions(junction, junction.Position + direction, direction, 1, false);
            }
        }
    }
}

internal abstract class LongestPathResolver
{
    private static readonly Dictionary<char, Coor> s_directions = new()
    {
        { '<', Coor.Left },
        { '>', Coor.Right },
        { '^', Coor.Up },
        { 'v', Coor.Down },
    };

    protected readonly char[,] Map;
    protected readonly Coor Start;
    protected readonly Coor End;
    protected readonly Dictionary<Coor<int>, Slope> Slopes;
    protected readonly Dictionary<Coor<int>, Junction> Junctions;

    public LongestPathResolver(char[,] map)
    {
        Map = map;

        Start = Enumerable.Range(0, map.Width())
            .Where(col => map[0, col] == '.')
            .Select(col => new Coor(0, col))
            .Single();

        End = Enumerable.Range(0, map.Width())
            .Where(col => map[map.Height() - 1, col] == '.')
            .Select(col => new Coor(map.Height() - 1, col))
            .Single();

        Slopes = map.AllCoordinates()
            .Where(coor => s_directions.ContainsKey(map.Get(coor)))
            .ToDictionary(c => c, c => new Slope(s_directions[map.Get(c)]));

        Junctions = new()
        {
            { Start, new Junction(Start) },
            { End, new Junction(End) },
        };

        FindJunctions(Start + Coor.Down, Coor.Down);
        ConnectAllJunctions();
    }

    public abstract int FindLongestPath();

    // Connects all junctions to other junctions we can travel to
    protected abstract void ConnectAllJunctions();

    // Finds all junctions reachable from source junction
    protected void ConnectJunctions(Junction source, Coor position, Coor cameFromDirection, int distance, bool recursive)
    {
        if (distance > 0 && Junctions.TryGetValue(position, out var junction))
        {
            source.Neighbours.Add((junction, distance));

            if (recursive)
            {
                ConnectJunctions(junction, position, cameFromDirection, 0, true);
            }

            return;
        }

        var possibleDirections = GetPossibleDirections(position, cameFromDirection).ToList();
        foreach (var newDirection in possibleDirections)
        {
            ConnectJunctions(source, position + newDirection, newDirection, distance + 1, recursive);
        }
    }

    // Walks the map adhering to rules and finds all junctions
    private void FindJunctions(Coor position, Coor cameFromDirection)
    {
        if (Junctions.ContainsKey(position))
        {
            return;
        }

        var possibleDirections = GetPossibleDirections(position, cameFromDirection).ToList();
        if (possibleDirections.Count > 1)
        {
            Map.Set(position, '+');
            Junctions.Add(position, new Junction(position));
        }

        foreach (var newDirection in possibleDirections)
        {
            FindJunctions(position + newDirection, newDirection);
        }
    }

    // Finds all possible directions we can go from a given position
    // Won't go back or against a slope
    protected IEnumerable<Coor> GetPossibleDirections(Coor position, Coor cameFromDirection)
    {
        foreach (var newDirection in Coor.FourWayNeighbours)
        {
            if (newDirection.IsOpposite(cameFromDirection))
            {
                continue;
            }

            var newPosition = position + newDirection;
            if (!newPosition.InBoundsOf(Map))
            {
                continue;
            }

            var c = Map.Get(newPosition);
            if (c == '#')
            {
                continue;
            }

            if (c == '.' || c == '+' || Slopes.TryGetValue(newPosition, out var slope) && !slope.Direction.IsOpposite(newDirection))
            {
                yield return newDirection;
            }
        }
    }

    internal void Visualize(Coor? position1 = default, Coor? position2 = default)
    {
        Console.Clear();
        Map.Print(c =>
        {
            if (c == Start) return 'S';
            if (c == End) return 'E';
            if (Junctions.ContainsKey(c)) return '+';
            if (c == position1) return 'o';
            if (c == position2) return '*';
            return null;
        });
    }

    protected record Slope(Coor Direction);

    protected record Junction(Coor Position)
    {
        public List<(Junction Junction, int Distance)> Neighbours { get; } = [];
    }
}
