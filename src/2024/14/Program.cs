using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

const int height = 103;
const int width = 101;

var robots = Resources.GetInputFileLines()
    .Select(line => line.ParseNumbersOut())
    .Select(n => new Robot(new Coor(n[1], n[0]), new Coor(n[3], n[2]), width, height))
    .ToArray();

int second = 0;
int[,] quadrants = null!;
while (!IsEasterEgg(robots, width, height))
{
    ++second;
    for (int j = 0; j < robots.Length; ++j)
    {
        robots[j].Move();
    }

    if (second == 100)
    {
        quadrants = GetQuadrantCounts(robots, width, height);
    }
}

Console.WriteLine($"Part 1: {quadrants[1, 0] * quadrants[1, 1] * quadrants[0, 0] * quadrants[0, 1]}");
Console.WriteLine($"Part 2: {second}");

static bool IsEasterEgg(Robot[] robots, int width, int height)
{
    var border = 5;
    var inTheMiddle = robots.Count(
        r => r.Position.X >= border && r.Position.X < width - border
          && r.Position.Y >= border && r.Position.Y < height - border);

    if (inTheMiddle > robots.Length / 10 * 9)
    {
        new char[height, width].Print(c => !robots.Any(r => r.Position == c) ? '.' : (char)(robots.Count(r => r.Position == c) + '0'));
        return true;
    }

    return false;
}

static int[,] GetQuadrantCounts(Robot[] robots, int width, int height)
{
    int[,] quadrants = new int[2, 2];
    int xMid = (width + 1) / 2;
    int yMid = (height + 1) / 2;
    for (int j = 0; j < robots.Length; ++j)
    {
        var (y, x) = robots[j].Position;
        if (x == xMid - 1 || y == yMid - 1) continue;

        quadrants[x / xMid, y / yMid] += 1;
    }

    return quadrants;
}

file class Robot(Coor position, Coor velocity, int width, int height)
{
    private readonly Coor<int> _velocity = velocity;
    private readonly int _width = width;
    private readonly int _height = height;

    public Coor<int> Position { get; private set; } = position;

    public void Move()
    {
        Position += _velocity;
        if (Position.Y >= _height) Position = Position with { Y = Position.Y - _height };
        else if (Position.Y < 0) Position = Position with { Y = Position.Y + _height };
        if (Position.X >= _width) Position = Position with { X = Position.X - _width };
        else if (Position.X < 0) Position = Position with { X = Position.X + _width };
    }
}
