using AdventOfCode.Common;
using Common;

using Coor = AdventOfCode.Common.Coor<int>;

char[,] map = Resources.GetInputFileLines()
    .ParseAsArray();

Coor start = Enumerable.Range(0, map.Width())
    .Where(col => map[0, col] == '.')
    .Select(col => new Coor(0, col))
    .Single();

Coor end = Enumerable.Range(0, map.Width())
    .Where(col => map[map.Height() - 1, col] == '.')
    .Select(col => new Coor(map.Height() - 1, col))
    .Single();

var directions = new Dictionary<char, Coor>
{
    { '<', Coor.Left },
    { '>', Coor.Right },
    { '^', Coor.Up },
    { 'v', Coor.Down },
};

Dictionary<Coor<int>, Slope> slopes = map.AllCoordinates()
    .Where(coor => directions.ContainsKey(map.Get(coor)))
    .ToDictionary(c => c, c => new Slope(c, directions[map.Get(c)]));

FindJunctions(start + Coor.Down, Coor.Down);

Dictionary<Coor<int>, Junction> junctions = map.AllCoordinates()
    .Where(coor => map.Get(coor) == '+' || coor == start || coor == end)
    .ToDictionary(c => c, c => new Junction(c));

Visualize();

ConnectJunctions(junctions[start], start, Coor.Down, 0);

Console.Clear();
Visualize();



// Walks the map adhering to rules and finds all junctions
void FindJunctions(Coor position, Coor cameFromDirection)
{
    if (map.Get(position) == '+')
    {
        return;
    }

    var possibleDirections = GetPossibleDirections(position, cameFromDirection).ToList();
    if (possibleDirections.Count > 1)
    {
        map.Set(position, '+');
    }

    foreach (var newDirection in possibleDirections)
    {
        FindJunctions(position + newDirection, newDirection);
    }
}

// Connects all junctions to other junctions we can travel to
void ConnectJunctions(Junction source, Coor position, Coor cameFromDirection, int distance)
{
    if (distance > 0 && junctions.TryGetValue(position, out var junction))
    {
        source.Neighbours.Add((junction, distance));
        ConnectJunctions(junction, junction, cameFromDirection, 0);
        return;
    }

    var possibleDirections = GetPossibleDirections(position, cameFromDirection).ToList();
    foreach (var newDirection in possibleDirections)
    {
        Visualize(new Coor(source.Y, source.X), position + newDirection);
        Console.ReadLine();
        ConnectJunctions(source, position + newDirection, newDirection, distance + 1);
    }
}

// Finds all possible directions we can go from a given position
// Won't go back or against a slope
IEnumerable<Coor> GetPossibleDirections(Coor position, Coor cameFromDirection)
{
    foreach (var newDirection in Coor.FourWayNeighbours)
    {
        if (newDirection.IsOpposite(cameFromDirection))
        {
            continue;
        }

        var newPosition = position + newDirection;
        if (!newPosition.InBoundsOf(map))
        {
            continue;
        }

        var c = map.Get(newPosition);
        if (c == '#')
        {
            continue;
        }

        if (c == '.' || c == '+' || (slopes.TryGetValue(newPosition, out var slope) && !slope.Direction.IsOpposite(newDirection)))
        {
            yield return newDirection;
        }
    }
}

void Visualize(Coor? position1 = default, Coor? position2 = default)
{
    Console.Clear();
    map.Print(c =>
    {
        if (c == start) return 'S';
        if (c == end) return 'E';
        if (junctions.ContainsKey(c)) return '+';
        if (c == position1) return 'o';
        if (c == position2) return '*';
        return null;
    });
}

file record Slope(Coor position, Coor direction)
     : Coor(position.Y, position.X)
{
    public Coor Direction { get; } = direction;
}

file record Junction(Coor position)
    : Coor(position.Y, position.X)
{
    public List<(Junction Junction, int Distance)> Neighbours { get; } = [];
}
