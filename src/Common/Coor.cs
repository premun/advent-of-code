namespace Common;

public record Coor(int Y, int X)
{
    public static Coor operator -(Coor me, Coor other) => new(Y: me.Y - other.Y, X: me.X - other.X);
    public static Coor operator +(Coor me, Coor other) => new(Y: me.Y + other.Y, X: me.X + other.X);
}
