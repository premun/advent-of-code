namespace _16;

class BitReader
{
    private readonly string _source;

    public int Position { get; private set; } = 0;

    public BitReader(string source)
    {
        _source = ToBits(source);
    }

    public bool ReadBit() => _source[Position++] == '1';

    public string ReadBits(int numberOfBits)
    {
        var bits = _source.AsSpan(Position, numberOfBits).ToString();
        Position += numberOfBits;
        return bits;
    }

    public long ReadNumber(int numberOfBits) => Convert.ToInt64(ReadBits(numberOfBits), 2);

    /// <summary>
    /// Turns D2FE28 into 110100101111111000101000
    /// </summary>
    private static string ToBits(string input) => string.Join(string.Empty, input.Select(
        c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
}
