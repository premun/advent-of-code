using AdventOfCode._2021_18;
using AdventOfCode.Common;

var numbers = Resources.GetInputFileLines().Select(SnailfishParser.Parse).ToArray();

var part1 = numbers.Aggregate((acc, n) => acc + n).Magnitude;

var part2 =
    (from a in numbers
     from b in numbers
     where a != b
     select (a + b).Magnitude
    ).Max();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
