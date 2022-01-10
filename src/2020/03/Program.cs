using AdventOfCode.Common;

var map = Resources.GetResourceFileLines("input.txt").ParseAsJaggedArray(c => c == '#');

static int CountTrees(bool[][] map, Coor change)
{
    var current = change;
    var columns = map[0].Length;
    var trees = 0;

    while (current.Y < map.Length)
    {
        if (map[current.Y][current.X % columns])
        {
            trees++;
        }

        current += change;
    }

    return trees;
}

Console.WriteLine($"Part 1: {CountTrees(map, new Coor(1, 3))}");

var slopes = new[]
{
    new Coor(1, 1),
    new Coor(1, 3),
    new Coor(1, 5),
    new Coor(1, 7),
    new Coor(2, 1),
};

Console.WriteLine($"Part 2: {slopes.Select(coor => (long)CountTrees(map, coor)).Aggregate((t, acc) => t * acc)}");
