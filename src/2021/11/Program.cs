using AdventOfCode.Common;

var map = Resources.GetInputFileLines().ParseAsJaggedArray(c => c - '0');

static int Step(int[][] map)
{
    var flashingOctos = new Queue<Coor>();

    // First increase all by 1
    for (int row = 0; row < map.Length; row++)
    {
        for (int column = 0; column < map[row].Length; column++)
        {
            if (++map[row][column] == 10)
            {
                flashingOctos.Enqueue(new Coor(row, column));
            }
        }
    }

    // Flash
    var flashedOctos = new Queue<Coor>();
    while (flashingOctos.TryDequeue(out var flashing))
    {
        flashedOctos.Enqueue(flashing);

        foreach (var direction in Coor.NineWayNeighbours)
        {
            var around = flashing + direction;

            // Out of bounds
            if (around.Y < 0 || around.X < 0 || around.Y >= map.Length || around.X >= map[0].Length)
            {
                continue;
            }

            if (++map[around.Y][around.X] == 10)
            {
                flashingOctos.Enqueue(new Coor(around.Y, around.X));
            }
        }
    }

    var flashes = flashedOctos.Count;

    // Reset flashed to 0
    while (flashedOctos.TryDequeue(out var flashed))
    {
        map[flashed.Y][flashed.X] = 0;
    }

    return flashes;
}

static long Part1(int[][] map)
{
    long flashes = 0;

    for (int i = 0; i < 100; i++)
    {
        flashes += Step(map);
    }

    return flashes;
}

static int Part2(int[][] map)
{
    int step = 0;

    while (true)
    {
        step++;

        if (Step(map) == map.Length * map[0].Length)
        {
            return step;
        }
    }
}

Console.WriteLine($"Part 1: {Part1(map)}");

map = Resources.GetInputFileLines()
    .Select(line => line.Select(n => n - '0').ToArray())
    .ToArray();

Console.WriteLine($"Part 2: {Part2(map)}");
