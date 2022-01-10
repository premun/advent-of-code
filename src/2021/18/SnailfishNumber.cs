using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2021_18;

abstract class SnailfishNumber
{
    public abstract long Magnitude { get; }

    public Pair? Parent { get; set; }

    public abstract SnailfishNumber Clone();

    internal abstract bool TryExplode(int depth);

    internal abstract bool TrySplit();

    [MemberNotNullWhen(true, "Parent")]
    protected bool IsLeftChild => Parent?.Left == this;

    [MemberNotNullWhen(true, "Parent")]
    protected bool IsRightChild => Parent?.Right == this;

    internal abstract Literal FindLeaf(bool rightLeaf);

    protected void ReplaceSelf(SnailfishNumber newNode)
    {
        if (IsLeftChild)
        {
            Parent.Left = newNode;
        }
        else if (IsRightChild)
        {
            Parent.Right = newNode;
        }
    }

    public static SnailfishNumber operator +(SnailfishNumber first, SnailfishNumber second)
    {
        var result = new Pair(left: first.Clone(), right: second.Clone());
        result.Reduce();
        return result;
    }
}
