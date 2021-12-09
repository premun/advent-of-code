using Common;

var coordinates = Resources.GetResourceFileLines("input.txt")
    .Select(line =>
    {
        var coor = line.SplitBy(" -> ")
            .SelectMany(c => c.Split(','))
            .Select(int.Parse)
            .ToArray();

        return (Start: (X: coor[0], Y: coor[1]), End: (X: coor[2], Y: coor[3]));
    });

static int CountOverlaps(IEnumerable<((int X, int Y) Start, (int X, int Y) End)> coordinates, bool skipDiagonal = true)
{
    var xCoordinates = coordinates.SelectMany(c => new[] { c.Start.X, c.End.X });
    var yCoordinates = coordinates.SelectMany(c => new[] { c.Start.Y, c.End.Y });

    var min = (X: xCoordinates.Min(), Y: yCoordinates.Min());
    var max = (X: xCoordinates.Max(), Y: yCoordinates.Max());

    var map = new int[max.X - min.X + 1, max.Y - min.Y + 1];
    var overlapCount = 0;

    foreach (var coor in coordinates)
    {
        var step = (X: coor.End.X - coor.Start.X, Y: coor.End.Y - coor.Start.Y);

        if (step.X != 0)
        {
            step.X /= Math.Abs(step.X);
        }

        if (step.Y != 0)
        {
            step.Y /= Math.Abs(step.Y);
        }

        if (skipDiagonal && step.X != 0 && step.Y != 0)
        {
            // Non-horizontal + non-vertical line
            continue;
        }

        var current = (X: coor.Start.X - step.X, Y: coor.Start.Y - step.Y);

        do
        {
            current.X += step.X;
            current.Y += step.Y;

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
