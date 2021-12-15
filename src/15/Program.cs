using _15;
using Common;

var map = Resources.GetResourceFileLines("input.txt")
    .Select(line => line.Select(c => c - '0').ToArray())
    .ToArray();

var pathFinder = new PathFinder(map);

Console.WriteLine($"Part 1: {pathFinder.Search()}");
