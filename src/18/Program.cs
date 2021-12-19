using _18;
using Common;

var numbers = Resources.GetResourceFileLines("input.txt").Select(SnailfishNumber.FromString).ToArray();

var part1 = numbers.Aggregate((acc, n) => acc + n).Magnitude;

var part2 = numbers
    .AllCombinations(includeIdentities: false, orderSensitive: true)
    .Select(pair => (pair.Item1 + pair.Item2).Magnitude)
    .Max();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
