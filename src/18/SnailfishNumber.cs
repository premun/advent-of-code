namespace _18;

abstract record SnailfishNumber
{
    /* abstract long Magnitude { get; } */

    public Pair? Parent { get; set; }

    public abstract bool TryExplode(int depth);

    protected bool IsLeftChild() => Parent?.Left == this;

    protected bool IsRightChild() => Parent?.Right == this;
}

record Literal(long Value) : SnailfishNumber
{
    public override bool TryExplode(int depth) => false;
}

record Pair(SnailfishNumber Left, SnailfishNumber Right) : SnailfishNumber
{
    public void Reduce()
    {
        // Explode
        TryExplode(0);

        // Check for 10+
    }

    public override bool TryExplode(int depth)
    {
        if (depth == 4)
        {
            if (Left is not Literal left || Right is not Literal right)
            {
                throw new Exception("Depth 5 detected!");
            }

            Console.WriteLine($"Exploding [{left.Value}, {right.Value}]");
            return true;
        }

        return Left.TryExplode(depth + 1) || Right.TryExplode(depth + 1);
    }
}
