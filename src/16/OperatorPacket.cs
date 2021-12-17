namespace _16;

record OperatorPacket : Packet
{
    public IEnumerable<Packet> Subpackets { get; }

    public override long Value => RunOperation(TypeId, Subpackets.Select(s => s.Value));

    private OperatorPacket(int version, int typeId, IEnumerable<Packet> subpackets)
        : base(version, typeId)
    {
        Subpackets = subpackets;
    }

    public static OperatorPacket Parse(BitReader reader, int version, int typeId)
    {
        var lengthTypeId = reader.ReadBit();
        var subpackets = new List<Packet>();

        if (!lengthTypeId)
        {
            var totalLength = (int)reader.ReadNumber(15);
            var limit = reader.Position + totalLength;

            while (reader.Position < limit)
            {
                subpackets.Add(FromStream(reader));
            }
        }
        else
        {
            int numOfSubpackets = (int)reader.ReadNumber(11);

            for (int i = 0; i < numOfSubpackets; i++)
            {
                subpackets.Add(FromStream(reader));
            }
        }

        return new OperatorPacket(version, typeId, subpackets);
    }

    private static long RunOperation(int operationId, IEnumerable<long> values) => operationId switch
    {
        0 => values.Sum(),
        1 => values.Aggregate((acc, v) => acc * v),
        2 => values.Min(),
        3 => values.Max(),
        5 => values.First() > values.Last() ? 1 : 0,
        6 => values.First() < values.Last() ? 1 : 0,
        7 => values.First() == values.Last() ? 1 : 0,
        _ => throw new InvalidOperationException($"Unknown operation ID {operationId}"),
    };
}
