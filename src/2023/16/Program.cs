using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines().ParseAsArray();

var height = map.Height();
var width = map.Width();

int[] allScores =
[
    .. Enumerable.Range(0, height).Select(row => SimulateRays(new Coor(row, -1), Coor.Right)),
    .. Enumerable.Range(0, height).Select(row => SimulateRays(new Coor(row, width), Coor.Left)),
    .. Enumerable.Range(0, width).Select(col => SimulateRays(new Coor(-1, col), Coor.Down)),
    .. Enumerable.Range(0, width).Select(col => SimulateRays(new Coor(height, col), Coor.Up)),
];

Console.WriteLine($"Part 1: {SimulateRays(new Coor(0, -1), Coor.Right)}");
Console.WriteLine($"Part 2: {allScores.Max()}");

int SimulateRays(Coor startPosition, Coor startDirection)
{
    var energized = new bool[map!.Height(), map.Width()];
    var used = new bool[map.Height(), map.Width()];

    var rays = new Queue<Ray>([new Ray(startPosition, startDirection)]);
    int result = 0;

    while (rays.TryDequeue(out var ray))
    {
        var next = ray.Position + ray.Direction;
        if (!next.InBoundsOf(map))
        {
            continue;
        }

        if (!energized.Get(next))
        {
            energized.Set(next, true);
            result++;
        }

        switch (map.Get(next))
        {
            case '.':
                rays.Enqueue(new Ray(next, ray.Direction));
                break;

            case '/':
                rays.Enqueue(new Ray(next, Redirect(ray.Direction,
                    (Coor.Up, Coor.Right),
                    (Coor.Right, Coor.Up),
                    (Coor.Down, Coor.Left),
                    (Coor.Left, Coor.Down))));
                break;

            case '\\':
                rays.Enqueue(new Ray(next, Redirect(ray.Direction,
                    (Coor.Up, Coor.Left),
                    (Coor.Right, Coor.Down),
                    (Coor.Down, Coor.Right),
                    (Coor.Left, Coor.Up))));
                break;

            case '-':
                if (ray.Direction == Coor.Left || ray.Direction == Coor.Right)
                {
                    rays.Enqueue(new Ray(next, ray.Direction));
                }
                else
                {
                    if (used.Get(next))
                    {
                        // Do not spawn a ray we've spanwed before
                        break;
                    }

                    rays.Enqueue(new Ray(next, Coor.Left));
                    rays.Enqueue(new Ray(next, Coor.Right));
                    used.Set(next, true);
                }
                break;

            case '|':
                if (ray.Direction == Coor.Left || ray.Direction == Coor.Right)
                {
                    if (used.Get(next))
                    {
                        // Do not spawn a ray we've spanwed before
                        break;
                    }

                    rays.Enqueue(new Ray(next, Coor.Up));
                    rays.Enqueue(new Ray(next, Coor.Down));
                    used.Set(next, true);
                }
                else
                {
                    rays.Enqueue(new Ray(next, ray.Direction));
                }
                break;
        }
    }

    // map.Print(c => energized.Get(c) ? '#' : null);
    return result;
}

static Coor Redirect(Coor coor, params (Coor, Coor)[] transformations)
    => transformations.First(t => t.Item1 == coor).Item2;

file record struct Ray(Coor Position, Coor Direction);
