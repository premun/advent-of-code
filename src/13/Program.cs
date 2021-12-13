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
    var result = new HashSet<Coor>();

    foreach (var coor in coors)
    {
        result.Add(fold.Transform(coor));
    }

    return result;
}

Console.WriteLine($"Part 1: {Fold(points, folds.First()).Count}");
