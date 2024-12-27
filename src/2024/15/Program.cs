using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var input = Resources.GetInputFileLines();
var height = input.Count(i => i[0] == '#');
char[,] map = input.Take(height).ParseAsArray();
Coor[] instructions = input.Skip(height).SelectMany(c => c.Select(d => Coor.DirectionMap[d])).ToArray();

Coor start = map.AllCoordinates()
    .First(c => map.Get(c) == '@');

foreach (var dir in instructions)
{
    if (Move(map, start, dir))
    {
        start += dir;
    }

    //Console.Clear();
    //map.Print();
    //Console.ReadLine();
}


Console.WriteLine($"Part 1: {map.AllCoordinates().Select(c => map.Get(c) == 'O' ? c.Row * 100 + c.Col : 0).Sum()}");
Console.WriteLine($"Part 2: {""}");

static bool Move(char[,] map, Coor position, Coor direction)
{
    var targetPosition = position + direction;
    var current = map.Get(position);
    var targetTile = map.Get(targetPosition);
    switch (targetTile)
    {
        case '#':
            return false;
        case '.':
            map.Set(position, '.');
            map.Set(position + direction, current);
            return true;
        case 'O':
            if (Move(map, targetPosition, direction))
            {
                map.Set(position, '.');
                map.Set(position + direction, current);
                return true;
            }
            return false;
        default:
            throw new NotImplementedException();
    }
}
