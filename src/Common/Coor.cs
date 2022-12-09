namespace AdventOfCode.Common;

public record Coor(int Y, int X)
{
    public static Coor operator -(Coor me, Coor other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Coor operator +(Coor me, Coor other) => new(Y: me.Y + other.Y, X: me.X + other.X);

    public bool InBoundsOf(int[,] array) =>
        Y >= 0 && Y < array.GetLength(0) && X >= 0 && X < array.GetLength(1);

    public static readonly Coor[] FourWayNeighbours =
    {
        new (-1, 0),
        new (0, -1),
        new (0, 1),
        new (1, 0),
    };

    public static readonly Coor[] NineWayNeighbours =
    {
        new(-1, -1),
        new(-1, 0),
        new(-1, 1),
        new(0, -1),
        new(0, 1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
    };
}
