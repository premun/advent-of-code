namespace _16;

abstract record Packet(int Version, int TypeId)
{
    public static Packet FromHexadecimal(string input)
    {
        var reader = new BitReader(input);
        int version = reader.ReadNumber(3);
        int typeId = reader.ReadNumber(3);

        return typeId switch
        {
            4 => LiteralPacket.Parse(version, reader),
            _ => OperatorPacket.Parse(version, typeId, reader),
        };
    }
}

record LiteralPacket : Packet
{
    private const int DigitSize = 4;

    public int Value { get; }

    private LiteralPacket(int version, int value) : base(version, 4)
    {
        Value = value;
    }

    public static LiteralPacket Parse(int version, BitReader reader)
    {
        var value = 0;

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

record OperatorPacket : Packet
{
    public IEnumerable<Packet> Subpackets { get; }

    private OperatorPacket(int version, int typeId, IEnumerable<Packet> subpackets) : base(version, typeId)
    {
        Subpackets = subpackets;
    }

    public static OperatorPacket Parse(int version, int typeId, BitReader reader)
    {
        var lengthTypeId = reader.ReadBit();

        if (!lengthTypeId)
        {
            int totalLength = reader.ReadNumber(15);
        }
        else
        {
            int numOfSubpackets = reader.ReadNumber(11);

            for (int i = 0; i < numOfSubpackets; i++)
            {
                _sub
            }
        }

        return new OperatorPacket(version, typeId);
    }
}


