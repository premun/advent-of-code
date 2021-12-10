namespace _06;

record Coor(int X, int Y)
{
    public static Coor operator -(Coor me, Coor other) => new(me.X - other.X, me.Y - other.Y);
    public static Coor operator +(Coor me, Coor other) => new(me.X + other.X, me.Y + other.Y);
}
