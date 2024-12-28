using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var input = Resources.GetInputFileLines();
var height = input.Count(i => i[0] == '#');
char[,] map = input.Take(height).ParseAsArray();
Coor[] instructions = input.Skip(height).SelectMany(c => c.Select(d => Coor.DirectionMap[d])).ToArray();

var part1Sokoban = new Sokoban(map);
var part2Sokoban = new Sokoban(ScaleUpMap(map));

foreach (var dir in instructions)
{
    part1Sokoban.Move(dir);
    part2Sokoban.Move(dir);
}

Console.WriteLine($"Part 1: {part1Sokoban.GetGps()}");
Console.WriteLine($"Part 2: {part2Sokoban.GetGps()}");

static char[,] ScaleUpMap(char[,] map)
{
    char[,] scaledUp = new char[map.Height(), map.Width() * 2];
    foreach (Coor c in map.AllCoordinates())
    {
        (scaledUp[c.Row, c.Col * 2], scaledUp[c.Row, c.Col * 2 + 1]) = map.Get(c) switch
        {
            'O' => ('[', ']'),
            '@' => ('@', '.'),
            _ => (map.Get(c), map.Get(c)),
        };
    }

    return scaledUp;
}

file class Sokoban(char[,] map)
{
    private Coor _position = map.AllCoordinates().First(c => map.Get(c) == '@');
    private readonly char[,] _map = map;

    public bool Move(Coor direction)
    {
        if (Move(_position, direction))
        {
            _position += direction;
            return true;
        }

        return false;
    }

    public int GetGps() => _map.AllCoordinates()
        .Where(c => _map.Get(c) == 'O' || _map.Get(c) == '[')
        .Select(c => c.Row * 100 + c.Col)
        .Sum();

    private bool Move(Coor position, Coor direction)
    {
        var current = _map.Get(position);
        var targetPosition = position + direction;
        var target = _map.Get(targetPosition);
        switch (target)
        {
            case '#':
                return false;

            case '.':
                _map.Set(position, '.');
                _map.Set(targetPosition, current);
                return true;

            case 'O':
                if (Move(targetPosition, direction))
                {
                    _map.Set(position, '.');
                    _map.Set(targetPosition, current);
                    return true;
                }
                return false;

            case '[':
            case ']':
                if (direction == Coor.Left || direction == Coor.Right)
                {
                    if (Move(targetPosition, direction))
                    {
                        _map.Set(position, '.');
                        _map.Set(position + direction, current);
                        return true;
                    }
                    return false;
                }

                var otherTile = GetOtherBoxTile(targetPosition, target);
                if (CanMove(targetPosition, direction) && CanMove(otherTile, direction))
                {
                    MoveBigBox(otherTile, direction);
                    MoveBigBox(targetPosition, direction);
                    goto case '.';
                }

                return false;

            default:
                throw new InvalidOperationException();
        }
    }

    private bool CanMove(Coor position, Coor direction)
    {
        var target = _map.Get(position + direction);
        return target switch
        {
            '.' => true,
            '#' => false,
            'O' => CanMove(position + direction, direction),
            '[' or ']' => target == _map.Get(position)
                ? CanMove(position + direction, direction)
                : CanMove(position + direction, direction) && CanMove(GetOtherBoxTile(position + direction, target), direction),
            _ => throw new InvalidOperationException()
        };
    }

    private void MoveBigBox(Coor position, Coor direction)
    {
        var current = _map.Get(position);
        var targetPosition = position + direction;
        var target = _map.Get(targetPosition);

        switch (target)
        {
            case '.':
                _map.Set(position, '.');
                _map.Set(targetPosition, current);
                return;
            case ']':
                MoveBigBox(targetPosition, direction);
                MoveBigBox(targetPosition + Coor.Left, direction);
                goto case '.';
            case '[':
                MoveBigBox(targetPosition, direction);
                MoveBigBox(targetPosition + Coor.Right, direction);
                goto case '.';
            default:
                throw new InvalidOperationException();
        }
    }

    private static Coor GetOtherBoxTile(Coor position, char current)
    {
        return position + (current == ']' ? Coor.Left : Coor.Right);
    }
}
