namespace _18;

class Literal : SnailfishNumber
{
    public override long Magnitude => Value;

    public long Value { get; set; }

    public Literal(long value)
    {
        Value = value;
    }

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
}
