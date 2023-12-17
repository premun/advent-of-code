namespace AdventOfCode._2021_16;

class BitReader(string source)
{
    private readonly string _source = ToBits(source);

    public int Position { get; private set; } = 0;

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
