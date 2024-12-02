using System.Text.Json;
using AdventOfCode.Common;

var packets = Resources.GetInputFileLines()
    .Select(Packet.FromJson);

var inRightOrder = packets
    .GroupsOf(2)
    .Select((pair, index) => (pair, index))
    .Where(p => p.pair.First().CompareTo(p.pair.Last()) == -1)
    .Select(p => p.index + 1);

var divider1 = Packet.FromJson("[[2]]");
var divider2 = Packet.FromJson("[[6]]");

var sorted = packets
    .Append(divider1)
    .Append(divider2)
    .Order()
    .ToList();

Console.WriteLine($"Part 1: {inRightOrder.Sum()}");
Console.WriteLine($"Part 2: {(sorted.IndexOf(divider1) + 1)* (sorted.IndexOf(divider2) + 1)}");

abstract record Packet : IComparable
{
    public static Packet FromJson(string json)
        => FromJson((JsonElement)JsonSerializer.Deserialize<object>(json)!);

    public static Packet FromJson(JsonElement element)
        => element.ValueKind == JsonValueKind.Array
            ? new List(element.EnumerateArray().Select(FromJson).ToArray())
            : new Number(element.GetInt32());

    public int CompareTo(object? other)
    {
        var p1 = this;
        var p2 = other as Packet ?? throw new Exception("Not a packet");

        // If both values are integers, the lower integer should come first
        if (p1 is Number n1 && p2 is Number n2)
        {
            return n1.Value.CompareTo(n2.Value);
        }

        // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value
        List l1 = p1 is Number n ? new List([n]) : (List)p1;
        List l2 = p2 is Number m ? new List([m]) : (List)p2;

        // If both values are lists, compare the first value of each list, then the second value, and so on
        for (int i = 0;; i++)
        {
            var length1 = l1.Values.Length;
            var length2 = l2.Values.Length;

            // If the lists are the same length and no comparison makes a decision about the order,
            // continue checking the next part of the input
            if (length1 == length2 && length1 == i)
            {
                return 0;
            }

            if (i < length1 && i < length2)
            {
                var result = l1.Values[i].CompareTo(l2.Values[i]);
                if (result != 0)
                {
                    return result;
                }

                continue;
            }

            // If the left list runs out of items first, the inputs are in the right order
            return length1.CompareTo(length2);
        }
    }
}

record Number(int Value) : Packet;
record List(Packet[] Values) : Packet;
