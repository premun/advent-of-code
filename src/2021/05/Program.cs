using AdventOfCode.Common;

var coordinates = Resources.GetResourceFileLines("input.txt")
    .Select(line =>
    {
        var coor = line.SplitBy(" -> ")
            .SelectMany(c => c.Split(','))
            .Select(int.Parse)
            .ToArray();

        return (Start: new Coor(coor[0], coor[1]), End: new Coor(coor[2], coor[3]));
    });

static long CountOverlaps(IEnumerable<(Coor Start, Coor End)> coordinates, bool skipDiagonal = true)
{
    var xCoordinates = coordinates.SelectMany(c => new[] { c.Start.X, c.End.X });
    var yCoordinates = coordinates.SelectMany(c => new[] { c.Start.Y, c.End.Y });

    var min = new Coor(xCoordinates.Min(), yCoordinates.Min());
    var max = new Coor(xCoordinates.Max(), yCoordinates.Max());

    var map = new int[max.X - min.X + 1, max.Y - min.Y + 1];
    var overlapCount = 0L;

    foreach (var coor in coordinates)
    {
        var step = coor.End - coor.Start;

        if (step.X != 0)
        {
            step = step with { X = step.X / Math.Abs(step.X) };
        }

        if (step.Y != 0)
        {
            step = step with { Y = step.Y / Math.Abs(step.Y) };
        }

        if (skipDiagonal && step.X != 0 && step.Y != 0)
        {
            // Non-horizontal + non-vertical line
            continue;
        }

        var current = coor.Start - step;

        do
        {
            current += step;

            var x = current.X - min.X;
            var y = current.Y - min.Y;

            map[x, y] += 1;

            if (map[x, y] == 2)
            {
                overlapCount++;
            }
        } while (current != coor.End);
    }

    return overlapCount;
}

Console.WriteLine($"Part 1: {CountOverlaps(coordinates, true)} overlaps");
Console.WriteLine($"Part 2: {CountOverlaps(coordinates, false)} overlaps");
