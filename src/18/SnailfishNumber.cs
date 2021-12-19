using System.Diagnostics.CodeAnalysis;

namespace _18;

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
            return;
        }

        if (IsRightChild)
        {
            Parent.Right = newNode;
            return;
        }
    }

    public static SnailfishNumber operator +(SnailfishNumber first, SnailfishNumber second)
    {
        var addedNumber = new Pair(left: first.Clone(), right: second.Clone());
        addedNumber.Reduce();
        return addedNumber;
    }

    public static SnailfishNumber FromString(string number) => new SnailfishParser().Parse(number);
}
