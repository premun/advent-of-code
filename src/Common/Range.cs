using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Common;

public record Range(long Start, long End)
{
    public bool Contains(long number) => !(number < Start || number > End);

    public bool TryIntersect(Range other, [NotNullWhen(true)] out Range? result)
    {
        result = Intersect(other);
        return result != null;
    }

    public Range? Intersect(Range other)
    {
        var first = this;
        var second = other;

        if (other.Start < Start)
        {
            (first, second) = (second, first);
        }

        // First contains all of second
        if (first.End >= second.End)
        {
            return second;
        }

        // First ends before the second
        if (first.End >= second.Start)
        {
            return new Range(second.Start, first.End);
        }

        return null;
    }

    public IEnumerable<long> Enumerate()
    {
        for (var i = Start; i <= End; ++i)
        {
            yield return i;
        }
    }

    public override string ToString()
        => $"[{(Start == long.MinValue ? "-" : Start)}, {(End == long.MaxValue ? "+" : End)}]";
}
