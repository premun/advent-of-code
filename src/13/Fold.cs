namespace _13;

abstract record Fold
{
    public abstract Coor Transform(Coor coor);
}

record XFold(int Position) : Fold
{
    public override Coor Transform(Coor coor)
    {
        if (coor.Column > Position)
        {
            coor = coor with
            {
                Column = Position - (coor.Column - Position)
            };
        }

        return coor;
    }
}

record YFold(int Position) : Fold
{
    public override Coor Transform(Coor coor)
    {
        if (coor.Row > Position)
        {
            coor = coor with
            {
                Row = Position - (coor.Row - Position)
            };
        }

        return coor;
    }
}
