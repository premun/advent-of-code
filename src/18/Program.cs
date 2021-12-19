using _18;
using Common;

var number = Resources.GetResourceFileLines("input.txt")
    .Select(SnailfishNumber.FromString)
    .Aggregate((acc, n) => acc + n);

Console.WriteLine($"Part 1: {number.Magnitude}");
