﻿namespace _18;

class Pair : SnailfishNumber
{
    private SnailfishNumber _left;
    private SnailfishNumber _right;

    public override long Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

    public SnailfishNumber Left
    {
        get => _left;
        set
        {
            _left = value;
            value.Parent = this;
        }
    }

    public SnailfishNumber Right
    {
        get => _right;
        set
        {
            _right = value;
            value.Parent = this;
        }
    }

    public Pair(SnailfishNumber left, SnailfishNumber right)
    {
        _left = left;   // I have to do this for nullability???
        _right = Right; // Otherwise I get a warning

        Left = left;
        Right = right;
    }

    public void Reduce()
    {
        while (true)
        {
            if (TryExplode(0))
            {
                continue;
            }

            if (TrySplit())
            {
                continue;
            }

            break;
        }
    }

    internal override bool TryExplode(int depth)
    {
        if (depth == 4)
        {
            if (Left is not Literal left || Right is not Literal right)
            {
                throw new Exception("Depth 5 detected!");
            }

            var leftNeighbour = FindLeftNeighbour();
            var rightNeighbour = FindRightNeighbour();

            if (rightNeighbour != null)
            {
                if (leftNeighbour == null)
                {
                    rightNeighbour.Value += right.Value;
                    ReplaceSelf(new Literal(0));
                }
                else
                {
                    // Can explode to both sides
                    rightNeighbour.Value += right.Value;
                    leftNeighbour.Value += left.Value;
                    ReplaceSelf(new Literal(0));
                }
            }
            else
            {
                if (leftNeighbour != null)
                {
                    leftNeighbour.Value += left.Value;
                    ReplaceSelf(new Literal(0));
                }
                else
                {
                    // Nowhere to explode, no-op
                    return false;
                }
            }

            return true;
        }

        return Left.TryExplode(depth + 1) || Right.TryExplode(depth + 1);
    }

    internal override bool TrySplit() => Left.TrySplit() || Right.TrySplit();

    private Literal? FindRightNeighbour()
    {
        var node = this;
        while (node != null)
        {
            if (node.IsLeftChild)
            {
                return node.Parent.Right.FindLeaf(rightLeaf: false);
            }

            node = node.Parent;
        }

        return null;
    }

    private Literal? FindLeftNeighbour()
    {
        var node = this;
        while (node != null)
        {
            if (node.IsRightChild)
            {
                return node.Parent.Left.FindLeaf(rightLeaf: true);
            }

            node = node.Parent;
        }

        return null;
    }

    internal override Literal FindLeaf(bool rightLeaf)
    {
        SnailfishNumber node = this;
        while (true)
        {
            switch (node)
            {
                case Literal literal:
                    return literal;

                case Pair pair:
                    node = rightLeaf ? pair.Right : pair.Left;
                    break;

                default:
                    throw new Exception("???");
            }
        }
    }

    public override string ToString() => $"[{Left},{Right}]";
}