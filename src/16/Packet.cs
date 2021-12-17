namespace _16;

abstract record Packet(int Version, int TypeId)
{
    public abstract long Value { get; }

    public static Packet FromStream(BitReader reader)
    {
        var version = (int)reader.ReadNumber(3);
        var typeId = (int)reader.ReadNumber(3);

        if (typeId == 4)
        {
            return LiteralPacket.Parse(reader, version);
        }

        return OperatorPacket.Parse(reader, version, typeId);
    }
}
