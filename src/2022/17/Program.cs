using AdventOfCode._2022_17;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

bool[] jetPattern = Resources.GetInputFileLines()
    .First()
    .Select(c => c == '>')
    .ToArray();

var chamber = new Chamber(4, 7, jetPattern);
Console.WriteLine(chamber.Simulate(2022));

