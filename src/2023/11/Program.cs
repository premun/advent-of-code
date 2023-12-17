using AdventOfCode.Common;
using Common;

bool[,] map = Resources.GetInputFileLines()
    .ParseAsArray(c => c == '#');

var rows = map.Height();
var cols = map.Width();

List<Coor<long>> stars =
    [.. from row in Enumerable.Range(0, rows)
    from col in Enumerable.Range(0, cols)
    where map[row, col]
    select new Coor<long>(row, col)];

var part1sum = stars
    .AllCombinations(includeIdentities: false, orderSensitive: false)
    .Select(p => p.Item1.ManhattanDistance(p.Item2))
    .Sum() / 2;
var part2sum = part1sum;

var emptyRows = Enumerable.Range(0, cols)
    .Where(row => stars.All(s => s.Row != row));

foreach (var row in emptyRows)
{
    var starsTop = stars.Count(s => s.Row < row);
    var starsBelow = stars.Count(s => s.Row > row);
    var connections = Math.Abs(starsTop * starsBelow);
    part1sum += connections;
    part2sum += (1_000_000L - 1) * connections;
}

var emptyCols = Enumerable.Range(0, rows)
    .Where(col => stars.All(s => s.Col != col));

foreach (var col in emptyCols)
{
    var starsLeft = stars.Count(s => s.Col < col);
    var starsRight = stars.Count(s => s.Col > col);
    var connections = Math.Abs(starsLeft * starsRight);
    part1sum += connections;
    part2sum += (1_000_000L - 1) * connections;
}

Console.WriteLine($"Part 1: {part1sum}");
Console.WriteLine($"Part 2: {part2sum}");
