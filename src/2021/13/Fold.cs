using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2021_13;

abstract record Fold
{
    public abstract Coor Transform(Coor coor);
}

record XFold(int Position) : Fold
{
    public override Coor Transform(Coor coor)
    {
        if (coor.X > Position)
        {
            coor = coor with
            {
                X = Position - (coor.X - Position)
            };
        }

        return coor;
    }
}

record YFold(int Position) : Fold
{
    public override Coor Transform(Coor coor)
    {
        if (coor.Y > Position)
        {
            coor = coor with
            {
                Y = Position - (coor.Y - Position)
            };
        }

        return coor;
    }
}
