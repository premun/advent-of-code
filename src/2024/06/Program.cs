﻿using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

char[,] map = Resources.GetInputFileLines().ParseAsArray();
Coor start = map.AllCoordinates().First(c => map.Get(c) == '^');

Simulate(map, start);
var traversedPath = map.AllCoordinates()
    .Where(c => map.Get(c) != '.' && map.Get(c) != '#')
    .ToList();

var possibleObstaclePositions = 0;
foreach (var path in traversedPath.Where(c => c != start))
{
    try
    {
        var newMap = Resources.GetInputFileLines().ParseAsArray();
        newMap.Set(path, '#');
        Simulate(newMap, start);
    }
    catch (LoopException)
    {
        possibleObstaclePositions++;
    }
}

Console.WriteLine($"Part 1: {traversedPath.Count}");
Console.WriteLine($"Part 2: {possibleObstaclePositions}");

static void Simulate(char[,] map, Coor position)
{
    var direction = Coor.Up;

    while (true)
    {
        var current = map.Get(position);
        map.Set(position, current switch
        {
            '.' => '0',
            _ => (char)(current + 1),
        });

        var next = position + direction;
        if (!next.InBoundsOf(map))
        {
            break;
        }

        if (map.Get(next) == '#')
        {
            direction = direction.TurnRight();
            continue;
        }

        // We can only come to a field twice (from side and from top/down)
        // If we come a third time, it's a loop
        if (map.Get(next) != '.' && map.Get(next) == '3')
        {
            throw new LoopException();
        }

        position = next;
    }
}

file class LoopException : Exception;
