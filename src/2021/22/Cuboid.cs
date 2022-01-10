using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2021_22;

record Dim(int Min, int Max)
{
    public int Length { get; } = Max - Min + 1;

    public override string ToString() => $"{Min}..{Max}";

    public IEnumerable<int> Points => Enumerable.Range(Min, Length);
}

record Cuboid(Dim X, Dim Y, Dim Z)
{
    public ulong Volume { get; } = (ulong)X.Length * (ulong)Y.Length * (ulong)Z.Length;

    public bool TryIntersect(Cuboid other, [NotNullWhen(true)] out Cuboid? intersection)
    {
        if (TryIntersect(X, other.X, out var x) &&
            TryIntersect(Y, other.Y, out var y) &&
            TryIntersect(Z, other.Z, out var z))
        {
            intersection = new Cuboid(X: x, Y: y, Z: z);
            return true;
        }

        intersection = null;
        return false;
    }

    private static bool TryIntersect(Dim first, Dim second, [NotNullWhen(true)] out Dim? intersection)
    {
        if (first.Min > second.Min)
        {
            (first, second) = (second, first);
        }

        if (first.Max < second.Min)
        {
            intersection = null;
            return false;
        }

        intersection = new(second.Min, Math.Min(first.Max, second.Max));
        return true;
    }

    public override string ToString() => $"{X}  {Y}  {Z}";
}
