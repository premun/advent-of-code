using _17;
using Common;

var input = Resources.GetResourceFileLines("input.txt").First();

var coordinates = input
    .Substring("target area: x=".Length)
    .Split(", y=")
    .SelectMany(s => s.Split(".."))
    .Select(int.Parse)
    .ToArray();

Coor from = new Coor(X: Math.Min(coordinates[0], coordinates[1]), Y: Math.Max(coordinates[2], coordinates[3]));
Coor to = new Coor(X: Math.Max(coordinates[0], coordinates[1]), Y: Math.Min(coordinates[2], coordinates[3]));

// Minimal speed to reach the nearest part of the target (gives us the most time to fire up)
var minXVelocity = (int)Math.Floor((1 + Math.Sqrt(1 + from.X * 8)) / 2);

bool IsHit(Coor speed, out int maxHeight)
{
    var position = new Coor(0, 0);
    maxHeight = 0;

    while (position.X < to.X && position.Y > to.Y)
    {
        position += speed;
        speed = speed with
        {
            X = speed.X > 0 ? speed.X - 1 : 0,
            Y = speed.Y - 1,
        };

        maxHeight = Math.Max(maxHeight, position.Y);

        if (position.X >= from.X && position.Y <= from.Y && position.X <= to.X && position.Y >= to.Y)
        {
            return true;
        }
    }

    return false;
}

int GetMaxHeight()
{
    int maxHeight = 0;

    for (int yVelocity = 0; yVelocity <= -to.Y; yVelocity++)
    {
        if (IsHit(new Coor(X: minXVelocity, Y: yVelocity), out var max))
        {
            maxHeight = Math.Max(maxHeight, max);
        }
    }

    return maxHeight;
}

int GetPossibleVelocityCount()
{
    int hitCount = 0;

    for (int xVelocity = minXVelocity; xVelocity <= to.X; xVelocity++)
    for (int yVelocity = to.Y; yVelocity <= -to.Y; yVelocity++)
    {
        if (IsHit(new Coor(X: xVelocity, Y: yVelocity), out _))
        {
            hitCount++;
        }
    }

    return hitCount;
}

Console.WriteLine($"Part 1: {GetMaxHeight()}");
Console.WriteLine($"Part 1: {GetPossibleVelocityCount()}");
