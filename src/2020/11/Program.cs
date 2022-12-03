using AdventOfCode.Common;

var map = Resources.GetInputFileLines().ParseAsArray();

static bool TrySimulate(
    char[,] map,
    char[,] result,
    Func<Coor, List<Coor>> getNeighbours,
    int requiredNeighbourCount)
{
    var sizeY = map.GetLength(0);
    var sizeX = map.GetLength(1);
    var changed = false;

    for (int y = 0; y < sizeY; y++)
    {
        for (int x = 0; x < sizeX; x++)
        {
            if (map[y, x] == '.')
            {
                continue;
            }

            int neighbourCount = 0;
            var neighbours = getNeighbours(new Coor(y, x));

            for (int n = 0; n < neighbours.Count && neighbourCount < requiredNeighbourCount; n++)
            {
                if (map[neighbours[n].Y, neighbours[n].X] == '#')
                {
                    ++neighbourCount;
                }
            }

            if (map[y, x] == 'L' && neighbourCount == 0)
            {
                result[y, x] = '#';
                changed = true;
            }
            else if (map[y, x] == '#' && neighbourCount >= requiredNeighbourCount)
            {
                result[y, x] = 'L';
                changed = true;
            }
            else
            {
                result[y, x] = map[y, x];
            }
        }
    }

    return changed;
}

static int FindOccupiedSeats(char[,] map, Func<Coor, List<Coor>> getNeighbours, int requiredNeighbourCount)
{
    var map2 = (char[,])map.Clone();

    while (TrySimulate(map, map2, getNeighbours, requiredNeighbourCount))
    {
        (map, map2) = (map2, map);
    }

    return
        (from y in Enumerable.Range(0, map2.GetLength(0))
        from x in Enumerable.Range(0, map2.GetLength(1))
        select map2[y, x] == '#').Count(x => x);
}

static int Part1(char[,] map)
{
    var sizeY = map.GetLength(0);
    var sizeX = map.GetLength(1);

    var neighbours = new Coor[]
    {
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
    };

    return FindOccupiedSeats(
        (char[,])map.Clone(),
        c => neighbours
            .Select(n => c + n)
            .Where(n => n.X >= 0 && n.Y >= 0 && n.X < sizeX && n.Y < sizeY)
            .ToList(),
        4);
}

static int Part2(char[,] map)
{
    var sizeY = map.GetLength(0);
    var sizeX = map.GetLength(1);
    var neighbourMap = new List<Coor>[sizeY, sizeX];

    var directions = new Coor[]
    {
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
    };

    for (int y = 0; y < sizeY; y++)
    {
        for (int x = 0; x < sizeX; x++)
        {
            var neighbours = new List<Coor>();
            for (int d = 0; d < 8; d++)
            {
                var n = new Coor(y, x) + directions[d];

                while (n.X >= 0 && n.Y >= 0 && n.X < sizeX && n.Y < sizeY)
                {
                    if (map[n.Y, n.X] != '.')
                    {
                        neighbours.Add(n);
                        break;
                    }

                    n += directions[d];
                }
            }

            neighbourMap[y, x] = neighbours;
        }
    }

    return FindOccupiedSeats((char[,])map.Clone(), c => neighbourMap[c.Y, c.X], 5);
}

Console.WriteLine($"Part 1: {Part1(map)}");
Console.WriteLine($"Part 2: {Part2(map)}");
