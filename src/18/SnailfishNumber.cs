using System.Diagnostics.CodeAnalysis;

namespace _18;

abstract class SnailfishNumber
{
    public abstract long Magnitude { get; }

    public Pair? Parent { get; set; }

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

        throw new InvalidOperationException();
    }

    public static SnailfishNumber operator +(SnailfishNumber first, SnailfishNumber second)
    {
        var addedNumber = new Pair(left: first, right: second);
        addedNumber.Reduce();
        return addedNumber;
    }

    public static SnailfishNumber FromString(string number) => new SnailfishParser().Parse(number);
}
