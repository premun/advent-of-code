using AdventOfCode._2021_09;
using AdventOfCode.Common;

var heatMap = Resources.GetInputFileLines().ParseAsJaggedArray(c => c - '0');

static long Part1(int[][] heatMap)
{
    long result = 0;

    for (int row = 0; row < heatMap.Length; row++)
    {
        for (int column = 0; column < heatMap[row].Length; column++)
        {
            int current = heatMap[row][column];

            if (row > 0 && heatMap[row - 1][column] <= current)
            {
                continue;
            }

            if (column > 0 && heatMap[row][column - 1] <= current)
            {
                continue;
            }

            if (column < heatMap[row].Length - 1 && heatMap[row][column + 1] <= current)
            {
                continue;
            }

            if (row < heatMap.Length - 1 && heatMap[row + 1][column] <= current)
            {
                continue;
            }

            result += 1 + current;
        }
    }

    return result;
}

static long Part2(int[][] heatMap)
{
    return new BasinMap(heatMap).FindBasins().Select(b => (long)b.Size).OrderByDescending(b => b).Take(3).Aggregate((acc, b) => acc * b);
}


Console.WriteLine($"Part 1: {Part1(heatMap)}");
Console.WriteLine($"Part 2: {Part2(heatMap)}");
