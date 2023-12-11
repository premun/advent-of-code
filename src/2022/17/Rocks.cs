using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2022_17;

abstract class Rock
{
    public int Height { get; }
    public int Width { get; }

    public IReadOnlyCollection<Coor> Positions { get; set; }

    public Rock(IReadOnlyCollection<Coor> positions)
    {
        Positions = positions;
        Height = positions.Max(p => p.Y) + 1;
        Width = positions.Max(p => p.X) - 1;
    }
}

class RowRock : Rock
{
    public RowRock() : base(new[]
    {
    new Coor(0, 2), new Coor(0, 3), new Coor(0, 4), new Coor(0, 5),
})
    { }
}

class CrossRock : Rock
{
    public CrossRock() : base(new[]
    {
                    new Coor(0, 3),
    new Coor(1, 2), new Coor(1, 3), new Coor(1, 4),
                    new Coor(2, 3),
})
    { }
}

class LRock : Rock
{
    public LRock() : base(new[]
    {
                                    new Coor(0, 4),
                                    new Coor(1, 4),
    new Coor(2, 2), new Coor(2, 3), new Coor(2, 4),
})
    { }
}

class ColumnRock : Rock
{
    public ColumnRock() : base(new[]
    {
    new Coor(0, 2),
    new Coor(1, 2),
    new Coor(2, 2),
    new Coor(3, 2),
})
    { }
}

class BoxRock : Rock
{
    public BoxRock() : base(new[]
    {
    new Coor(0, 2), new Coor(0, 3),
    new Coor(1, 2), new Coor(1, 3),
})
    { }
}
