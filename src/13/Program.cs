using _13;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

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
    foreach (var point in coors.OrderBy(p => p.Row).ThenBy(p => p.Column))
    {
        for (int i = last.Row; i < point.Row; i++)
        {
            Console.WriteLine();
            last = new Coor(0, point.Row);
        }

        for (int i = last.Column; i < point.Column; i++)
        {
            Console.Write(' ');
        }

        Console.Write('#');

        last = point with
        {
            Column = point.Column + 1,
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
