namespace _13;

record Coor(int Column, int Row)
{
    public static Coor operator -(Coor me, Coor other) => new(Row: me.Row - other.Row, Column: me.Column - other.Column);
    public static Coor operator +(Coor me, Coor other) => new(Row: me.Row + other.Row, Column: me.Column + other.Column);
}
