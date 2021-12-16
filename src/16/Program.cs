using _16;
using Common;

var input = "38006F45291200";// Resources.GetResourceFileLines("input.txt").First();

var packet = Packet.FromHexadecimal(input);

Console.WriteLine((packet as LiteralPacket)!.Value);
