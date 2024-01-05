using System.Text.RegularExpressions;
using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;
using Range = AdventOfCode.Common.Range;

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
        m.Groups["color"].Value))
    .ToList();

var borders = new List<Line>();
var lastEnd = Coor.Zero;
int minRow = int.MaxValue, minCol = int.MaxValue;
int maxRow = int.MinValue, maxCol = int.MinValue;

foreach (var instruction in instructions)
{
    var newEnd = lastEnd + new Coor(
        instruction.Distance * instruction.Direction.Y,
        instruction.Distance * instruction.Direction.X);

    borders.Add(Line.Create(lastEnd, newEnd));
    lastEnd = newEnd;

    minRow = Math.Min(minRow, lastEnd.Row);
    maxRow = Math.Max(maxRow, lastEnd.Row);
    minCol = Math.Min(minCol, lastEnd.Col);
    maxCol = Math.Max(maxCol, lastEnd.Col);
}

var verticals = borders
    .OfType<VerticalLine>()
    .OrderBy(b => b.Start.Col)
    .ToList();

var horizontals = borders
    .OfType<HorizontalLine>()
    .OrderBy(b => b.Start.Row)
    .ToList();

var horizontalMatches = new List<Range>[maxRow - minRow + 1];
for (int row = minRow; row <= maxRow; row++)
{
    var rowMatches = new List<Range>();

    var inside = false;
    var start = 0;
    var matchingVerticals = verticals.Where(b => b.IntersectsRow(row)).ToList();
    var matchingBorders = horizontals.Where(b => b.Start.Row == row).ToList();

    for (int i = 0; i < matchingVerticals.Count; i++)
    {
        var match = matchingVerticals[i];

        // Check if we are intersecting a horizontal border
        var matchingBorder = matchingBorders.FirstOrDefault(h => h.Start.Col == match.Col);
        if (matchingBorder != null)
        {
            if (inside)
            {
                rowMatches.Add(new Range(start, matchingBorder.Start.Col));
            }

            rowMatches.Add(new Range(matchingBorder.Start.Col, matchingBorder.End.Col));
            ++i; // Skip the next as it will be a perpendicular border
            inside = !inside;
            start = matchingBorder.End.Col;
            continue;
        }

        if (!inside)
        {
            start = match.Start.Col;
            inside = true;
        }
        else
        {
            rowMatches.Add(new Range(start, match.Start.Col));
        }
    }

    // Unify ranges
    int j = 1;
    while (j < rowMatches.Count)
    {
        var prev = rowMatches[j - 1];
        var current = rowMatches[j];
        if (current.Start - prev.End <= 1)
        {
            rowMatches.RemoveAt(j);
            rowMatches[j - 1] = new Range(prev.Start, current.End);
        }
        else
        {
            j++;
        }
    }

    horizontalMatches[row - minRow] = rowMatches;
}

var verticalMatches = new List<Range>[maxCol - minCol + 1];
for (int col = minCol; col <= maxCol; col++)
{
    var colMatches = new List<Range>();

    var inside = false;
    var start = 0;
    var matchingHorizontals = horizontals.Where(b => b.IntersectsCol(col)).ToList();
    var matchingBorders = verticals.Where(b => b.Start.Col == col).ToList();

    for (int i = 0; i < matchingHorizontals.Count; i++)
    {
        var match = matchingHorizontals[i];

        // Check if we are intersecting a horizontal border
        var matchingBorder = matchingBorders.FirstOrDefault(h => h.Start.Row == match.Row);
        if (matchingBorder != null)
        {
            if (inside)
            {
                colMatches.Add(new Range(start, matchingBorder.Start.Row));
            }

            colMatches.Add(new Range(matchingBorder.Start.Row, matchingBorder.End.Row));
            ++i; // Skip the next as it will be a perpendicular border
            inside = !inside;
            start = matchingBorder.End.Row;
            continue;
        }

        if (!inside)
        {
            start = match.Start.Row;
            inside = true;
        }
        else
        {
            colMatches.Add(new Range(start, match.Start.Row));
            inside = false;
        }
    }

    // Unify ranges
    int j = 1;
    while (j < colMatches.Count)
    {
        var prev = colMatches[j - 1];
        var current = colMatches[j];
        if (current.Start - prev.End <= 1)
        {
            colMatches.RemoveAt(j);
            colMatches[j - 1] = new Range(prev.Start, current.End);
        }
        else
        {
            j++;
        }
    }

    verticalMatches[col - minCol] = colMatches;
}

Console.WriteLine(horizontalMatches.SelectMany(m => m.Select(x => (long)x.End - x.Start + 1)).Sum());

abstract file record Line(Coor Start, Coor End)
{
    public bool IntersectsRow(int row) => row >= Start.Row && row <= End.Row;
    public bool IntersectsCol(int col) => col >= Start.Col && col <= End.Col;

    public static Line Create(Coor start, Coor end)
    {
        if (start.Row == end.Row)
        {
            return start.Col < end.Col
                ? new HorizontalLine(start, end)
                : new HorizontalLine(end, start);
        }
        else
        {
            return start.Row < end.Row
                ? new VerticalLine(start, end)
                : new VerticalLine(end, start);
        }
    }
}

file record HorizontalLine(Coor Start, Coor End) : Line(Start, End)
{
    public int Row => Start.Row;
}

file record VerticalLine(Coor Start, Coor End) : Line(Start, End)
{
    public int Col => Start.Col;
}

file record Instruction(Coor Direction, int Distance, string Color);

file record InvertedInstruction : Instruction
{
    public InvertedInstruction(string color)
        : base(
            color.Last() switch
            {
                '0' => Coor.Right,
                '1' => Coor.Down,
                '3' => Coor.Up,
                '2' => Coor.Left,
                _ => throw new Exception()
            },
            Convert.ToInt32(new string(color.Take(color.Length - 1).ToArray()), 16),
            color)
    {
    }
}
