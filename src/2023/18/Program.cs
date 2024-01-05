using System.Drawing;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var regex = new Regex(@"(?<direction>R|D|L|U) (?<distance>[0-9]+) \((?<color>#[0-9a-h]{6})\)");

var instructions = Resources.GetInputFileLines()
    .Select(line => regex.Match(line))
    .Where(m => m.Success)
    .Select(m => new Instruction(
        m.Groups["direction"].Value[0] switch
        {
            'R' => Coor.Right,
            'D' => Coor.Down,
            'U' => Coor.Up,
            'L' => Coor.Left,
            _ => throw new Exception()
        },
        int.Parse(m.Groups["distance"].Value),
        ColorTranslator.FromHtml(m.Groups["color"].Value)))
    .ToList();

List<Coor> trench = [];
int minRow = int.MaxValue, minCol = int.MaxValue;
int maxRow = int.MinValue, maxCol = int.MinValue;

foreach (var instruction in instructions)
{
    var last = trench.LastOrDefault() ?? Coor.Zero - instructions[0].Direction;
    for (var i = 0; i < instruction.Distance; i++)
    {
        last += instruction.Direction;
        trench.Add(last);

        minRow = Math.Min(minRow, last.Row);
        maxRow = Math.Max(maxRow, last.Row);
        minCol = Math.Min(minCol, last.Col);
        maxCol = Math.Max(maxCol, last.Col);
    }
}

// Make everything start at 1,1
if (Coor.Zero != (minRow, minCol))
{
    trench = trench
        .Select(c => c - (minRow, minCol) + (1, 1))
        .ToList();
}

// Make an additional row/col around the trench
var map = new char[maxRow - minRow + 3, maxCol - minCol + 3]
    .InitializeWith('.');

foreach (var c in trench)
{
    map.Set(c, '#');
}

//map.Print();

var queue = new Queue<Coor>();
queue.Enqueue(Coor.Zero);

var outside = 0;

while (queue.TryDequeue(out var c))
{
    if (map.Get(c) != '~')
    {
        outside++;
        map.Set(c, '~');
    }

    foreach (var n in c.GetFourWayNeighbours())
    {
        if (n.InBoundsOf(map) && map.Get(n) == '.')
        {
            map.Set(n, '_');
            queue.Enqueue(n);
        }
    }
}

Console.Clear();
map.Print();
Console.WriteLine();
Console.WriteLine($"Part 1: {(map.Height() * map.Width()) - outside}");

file record Instruction(Coor Direction, int Distance, Color Color);
