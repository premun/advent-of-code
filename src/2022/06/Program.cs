using AdventOfCode.Common;

var signal = Resources.GetInputFileLines().Single();

int GetMarker(int length) => Enumerable
    .Range(length, signal.Length - length)
    .First(index => signal.Substring(index - length, length).Distinct().Count() == length);

Console.WriteLine($"Part 1: {GetMarker(4)}");
Console.WriteLine($"Part 2: {GetMarker(14)}");
