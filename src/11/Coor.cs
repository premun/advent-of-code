namespace _11;

record Coor(int Row, int Column)
{
    public static Coor operator -(Coor me, Coor other) => new(me.Row - other.Row, me.Column - other.Column);
    public static Coor operator +(Coor me, Coor other) => new(me.Row + other.Row, me.Column + other.Column);
}
