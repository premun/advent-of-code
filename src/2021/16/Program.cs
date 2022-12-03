using AdventOfCode._2021_16;
using AdventOfCode.Common;

var input = Resources.GetInputFileLines().First();

var packet = Packet.ParseFromStream(new BitReader(input));

static int SumVersions(Packet packet)
{
    if (packet is LiteralPacket)
    {
        return packet.Version;
    }

    if (packet is OperatorPacket operatorPacket)
    {
        return packet.Version + operatorPacket.Subpackets.Select(SumVersions).Sum();
    }

    throw new Exception("???");
}

Console.WriteLine($"Part1: {SumVersions(packet)}");
Console.WriteLine($"Part2: {packet.Value}");
