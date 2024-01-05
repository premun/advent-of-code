using AdventOfCode._2023_23;
using AdventOfCode.Common;

var map = Resources.GetInputFileLines();

Console.WriteLine($"Part 1: {new LongestPathResolverWithSlopes(map).FindLongestPath()}");
Console.WriteLine($"Part 2: {new LongestPathResolverWithoutSlopes(map).FindLongestPath()}");
