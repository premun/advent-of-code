using Common;

namespace _23;

class AmphipodWorld
{
    private readonly Field[][] _map;
    private readonly int _sizeY;
    private readonly int _sizeX;

    public AmphipodWorld(char[][] world)
    {
        _sizeY = world.Length;
        _sizeX = world.Select(line => line.Length).Max();

        _map = new Field[_sizeY][];

        for (int y = 0; y < _sizeY; y++)
        {
            _map[y] = new Field[_sizeX];

            for (int x = 0; x < _sizeX; x++)
            {
                char c = x >= world[y].Length ? ' ' : world[y][x];

                Field field;

                switch (c)
                {
                    case (char)0:
                    case ' ':
                        field = new EmptyField();
                        break;

                    case '#':
                        field = new WallField();
                        break;

                    case '.':
                        field = Neighbourhs(y, x).Any(n => IsOccupied(world[n.Y][n.X]))
                            ? new RoomDoor()
                            : new HallwayField();
                        break;

                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                        var roomsToLeft = Enumerable.Range(0, x)
                            .Count(i => (_map[y][x] is OccupyableField f) && f.IsOccupied);

                        field = roomsToLeft switch
                        {
                            0 => new RoomAField(c),
                            1 => new RoomBField(c),
                            2 => new RoomCField(c),
                            3 => new RoomDField(c),
                            _ => throw new Exception("Too many rooms detected"),
                        };

                        break;

                    default:
                        throw new Exception($"Cannot recognize field {c}");
                }

                _map[y][x] = field;
            }
        }
    }

    public AmphipodWorld(Field[][] map)
    {
        _map = map;
        _sizeY = map.GetLength(0);
        _sizeX = map.GetLength(1);
    }

    public AmphipodWorld MoveAmphipod(Coor from, Coor to)
    {
        var newMap = new Field[_sizeY][];

        for (int y = 0; y < _sizeY; y++)
        {
            newMap[y] = new Field[_sizeX];

            for (int x = 0; x < _sizeX; x++)
            {
                if (y == from.Y && x == from.X)
                {
                    newMap[y][x] = _map[to.Y][to.X];
                }
                else if (y == to.Y && x == to.X)
                {
                    newMap[y][x] = _map[from.Y][from.X];
                }
                else
                {
                    newMap[y][x] = _map[y][x];
                }
            }
        }

        return new AmphipodWorld(newMap);
    }

    public bool IsFinished() => _map.SelectMany(row => row).All(field => field switch
    {
        RoomAField roomA => roomA.Occupant == 'A',
        RoomBField roomB => roomB.Occupant == 'B',
        RoomCField roomC => roomC.Occupant == 'C',
        RoomDField roomD => roomD.Occupant == 'D',
        OccupyableField room => room is RoomField || !room.IsOccupied,
        _ => true,
    });

    public Field Get(int Y, int X) => _map[Y][X];

    public Field Get(Coor coordinate) => _map[coordinate.Y][coordinate.X];

    public override string ToString() => new(
        Enumerable.Range(0, _sizeY).SelectMany(y =>
        Enumerable.Range(0, _sizeX).Select(x => _map[y][x].ToChar()).Append('\r').Append('\n'))
        .ToArray());

    public override int GetHashCode() => ToString().GetHashCode();

    public override bool Equals(object? obj)
    {
        if (base.Equals(obj))
        {
            return true;
        }

        if (obj is AmphipodWorld other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        return false;
    }

    private IEnumerable<Coor> Neighbourhs(int y, int x)
    {
        var coor = new Coor(y, x);

        return new[]
        {
            coor + new Coor(0, 1),
            coor + new Coor(1, 0),
            coor + new Coor(-1, 0),
            coor + new Coor(0, -1),
        }.Where(n => n.Y < _sizeY && n.X < _sizeX && n.Y >= 0 && n.X >= 0);
    }

    private static bool IsOccupied(char c) => c >= 'A' && c <= 'D';
}
