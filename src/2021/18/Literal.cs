namespace AdventOfCode._2021_18;

class Literal(long value) : SnailfishNumber
{
    public override long Magnitude => Value;

    public long Value { get; set; } = value;

    internal override bool TryExplode(int depth) => false;

    internal override bool TrySplit()
    {
        if (Value < 10)
        {
            return false;
        }

        ReplaceSelf(new Pair(
            left: new Literal(Value / 2),
            right: new Literal((Value + 1) / 2)));

        return true;
    }

    internal override Literal FindLeaf(bool rightLeaf) => this;

    public override string ToString() => Value.ToString();

    public override SnailfishNumber Clone() => new Literal(Value)
    {
        Parent = Parent
    };
}
