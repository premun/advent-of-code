namespace AdventOfCode._2021_16;

record LiteralPacket : Packet
{
    private const int DigitSize = 4;
    private readonly long _value;

    public override long Value => _value;

    private LiteralPacket(int version, long value) : base(version, 4)
    {
        _value = value;
    }

    public static LiteralPacket Parse(BitReader reader, int version)
    {
        var value = 0L;

        bool moreDigits;
        do
        {
            moreDigits = reader.ReadBit();
            value <<= DigitSize;
            value |= reader.ReadNumber(DigitSize);
        } while (moreDigits);

        return new LiteralPacket(version, value);
    }
}
