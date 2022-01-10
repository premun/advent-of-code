namespace AdventOfCode._2021_16;

abstract record Packet(int Version, int TypeId)
{
    public abstract long Value { get; }

    public static Packet ParseFromStream(BitReader reader)
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
