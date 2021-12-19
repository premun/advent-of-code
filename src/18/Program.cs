﻿using _18;
using Common;

var numbers = Resources.GetResourceFileLines("input.txt").Select(SnailfishParser.Parse).ToArray();

var part1 = numbers.Aggregate((acc, n) => acc + n).Magnitude;

var part2 =
    (from a in numbers
     from b in numbers
     where a != b
     select (a + b).Magnitude
    ).Max();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
