using Coor = AdventOfCode.Common.Coor<int>;
using Range = AdventOfCode.Common.Range;

internal class LavaLagoonMeasurer
{
    public static long MeasureLagoon(IEnumerable<Instruction> trenchInstructions)
    {
        var borders = new List<Line>();
        var lastEnd = Coor.Zero;
        int minRow = int.MaxValue, minCol = int.MaxValue;
        int maxRow = int.MinValue, maxCol = int.MinValue;

        foreach (var instruction in trenchInstructions)
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

        var horizontalStripes = new List<Range>[maxRow - minRow + 1];
        for (int row = minRow; row <= maxRow; row++)
        {
            var rowMatches = new List<Range>();

            var inside = false;
            var start = 0;
            var collidingVerticals = verticals.Where(b => row >= b.Start.Row && row <= b.End.Row).ToList();
            var matchingHorizontals = horizontals.Where(b => b.Start.Row == row).ToList();

            for (int i = 0; i < collidingVerticals.Count; i++)
            {
                var vertical = collidingVerticals[i];

                // Check if we are intersecting a horizontal border
                var horizontal = matchingHorizontals.FirstOrDefault(h => h.Start.Col == vertical.Start.Col);
                if (horizontal != null)
                {
                    if (inside)
                    {
                        // This situation:
                        // ####~~~~~####
                        //          ^
                        // (we need to add the ~ filling in between)
                        rowMatches.Add(new Range(start, horizontal.End.Col));
                    }
                    else
                    {
                        rowMatches.Add(new Range(horizontal.Start.Col, horizontal.End.Col));
                    }

                    // If we are not just brushing a top or bottom of the thing, we are crossing a boundary
                    var nextVertical = collidingVerticals[i + 1];
                    if ((vertical.Start.Row != row || nextVertical.Start.Row != row)
                        && (vertical.End.Row != row || nextVertical.End.Row != row))
                    {
                        inside = !inside;
                    }

                    ++i; // Skip the next as it will be a perpendicular border
                    start = horizontal.End.Col + 1;
                    continue;
                }

                if (!inside)
                {
                    start = vertical.Start.Col;
                    inside = true;
                }
                else
                {
                    rowMatches.Add(new Range(start, vertical.Start.Col));
                    inside = false;
                }
            }

            horizontalStripes[row - minRow] = rowMatches;
        }

        var result = 0L;
        foreach (var row in horizontalStripes)
        {
            foreach (var stripe in row)
            {
                result += stripe.End - stripe.Start + 1;
            }
        }
        return result;
    }

    public static void Visualize(List<Range>[] horizontalStripes, int minCol)
    {
        for (int i = 0; i < horizontalStripes.Length; i++)
        {
            var last = 0;
            for (int j = 0; j < horizontalStripes[i].Count; j++)
            {
                var start = (int)horizontalStripes[i][j].Start - minCol;
                var end = (int)horizontalStripes[i][j].End - minCol;
                Console.Write(new string(' ', start - last));

                Console.Write('#');
                Console.Write(new string('#', end - start - 1));
                Console.Write('#');
                last = end;
            }

            Console.WriteLine();
        }
    }

    private abstract record Line(Coor Start, Coor End)
    {
        public static Line Create(Coor start, Coor end)
            => start.Row == end.Row
                ? start.Col < end.Col
                    ? new HorizontalLine(start, end)
                    : new HorizontalLine(end, start)
                : start.Row < end.Row
                    ? new VerticalLine(start, end)
                    : new VerticalLine(end, start);
    }

    private record HorizontalLine(Coor Start, Coor End) : Line(Start, End);
    
    private record VerticalLine(Coor Start, Coor End) : Line(Start, End);
}
