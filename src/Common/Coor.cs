namespace AdventOfCode.Common;

public record Coor(int Y, int X)
{
    public int Row => Y;
    public int Col => X;

    public static readonly Coor Zero = new(0, 0);
    public static readonly Coor One = new(1, 1);

    public static Coor operator -(Coor me, Coor other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Coor operator +(Coor me, Coor other) => new(Y: me.Y + other.Y, X: me.X + other.X);

    public bool InBoundsOf<T>(T[,] array)
        => Y >= 0 && Y < array.GetLength(0) && X >= 0 && X < array.GetLength(1);

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

    public static bool operator ==(Coor me, (int, int) other) => new Coor(other.Item1, other.Item2) == me;
    public static bool operator !=(Coor me, (int, int) other) => !(me == other);
    public static Coor operator +(Coor me, (int, int) other) => new(Y: me.Y + other.Item1, X: me.X + other.Item2);
    public static Coor operator -(Coor me, (int, int) other) => new(Y: me.Y - other.Item1, X: me.X - other.Item2);

    public static int ManhattanDistance(Coor me, Coor other) => Math.Abs(me.Y - other.Y) + Math.Abs(me.X - other.X);

    public int ManhattanDistance(Coor other) => ManhattanDistance(this, other);

    public override string ToString() => $"[{Y},{X}]";
}

public static class CoorExtensions
{
    public static void Visualize(this ICollection<Coor> coors, Coor? min, Coor? max)
    {
        min ??= new Coor(coors.Min(c => c.Y), coors.Min(c => c.X));
        max ??= new Coor(coors.Max(c => c.Y), coors.Max(c => c.X));

        var width = max.X - min.X + 1;
        var height = max.Y - min.Y + 1;

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Console.Write(coors.Contains(new(y, x)) ? '#' : '.');
            }

            Console.WriteLine();
        }
    }

    public static T Get<T>(this T[,] items, Coor coor)
        => items[coor.Y, coor.X];
    
    public static T Set<T>(this T[,] items, Coor coor, T value)
        => items[coor.Y, coor.X] = value;
}
