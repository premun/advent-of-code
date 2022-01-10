using _15;
using Common;

var map = Resources.GetResourceFileLines("input.txt").ParseAsJaggedArray(c => c - '0');

static int[][] Resize(int[][] map, int factor)
{
    int sizeY = map.Length;
    int sizeX = map[0].Length;

    var resizedMap = new int[sizeY * factor][];

    for (int y = 0; y < sizeY * factor; y++)
    {
        resizedMap[y] = new int[sizeX * factor];

        for (int x = 0; x < sizeX * factor; x++)
        {
            int original = map[y % sizeY][x % sizeX];
            int increase = (x / sizeY) + (y / sizeX);

            resizedMap[y][x] = original + increase;

            while (resizedMap[y][x] > 9)
            {
                resizedMap[y][x] -= 9;
            }
        }
    }

    return resizedMap;
}

var pathFinder = new PathFinder(map);
Console.WriteLine($"Part 1: {pathFinder.FindShortestPath()}");

pathFinder = new PathFinder(Resize(map, 5));
Console.WriteLine($"Part 2: {pathFinder.FindShortestPath()}");
