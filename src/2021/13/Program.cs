using AdventOfCode._2021_13;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var points = new HashSet<Coor>();
var folds = new List<Fold>();

foreach (var line in lines)
{
    if (line.StartsWith("fold along"))
    {
        var pieces = line.Split(' ').Last().Split('=');

        switch (pieces.First())
        {
            case "x":
                folds.Add(new XFold(int.Parse(pieces.Last())));
                break;

            case "y":
                folds.Add(new YFold(int.Parse(pieces.Last())));
                break;

            default:
                throw new ArgumentException($"Invalid fold `{line}`");
        }

        continue;
    }

    var parts = line.Split(',').Select(int.Parse);
    points.Add(new Coor(parts.First(), parts.Last()));
}

static HashSet<Coor> Fold(HashSet<Coor> coors, Fold fold)
{
    return coors.Select(coor => fold.Transform(coor)).ToHashSet();
}

static void Display(HashSet<Coor> coors)
{
    var last = new Coor(0, 0);
    foreach (var point in coors.OrderBy(p => p.Y).ThenBy(p => p.X))
    {
        for (int i = last.Y; i < point.Y; i++)
        {
            Console.WriteLine();
            last = new Coor(0, point.Y);
        }

        for (int i = last.X; i < point.X; i++)
        {
            Console.Write(' ');
        }

        Console.Write('#');

        last = point with
        {
            X = point.X + 1,
        };
    }
}

Console.WriteLine($"Part 1: {Fold(points, folds.First()).Count}");

foreach (var fold in folds)
{
    points = Fold(points, fold);
}

Console.WriteLine($"Part 2: {points.Count}");

Display(points);
