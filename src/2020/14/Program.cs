using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

static void ProcessInput(string[] instructions, Action<string> onMask, Action<long, long> onStore)
{
    var maskRegex = new Regex(@"^mask = (?<mask>[X01]{36})$");
    var memoryRegex = new Regex(@"^mem\[(?<address>[0-9]+)\] = (?<value>[0-9]{1,36})$");

    foreach (var instruction in instructions)
    {
        var match = maskRegex.Match(instruction);
        if (match.Success)
        {
            onMask(match.Groups["mask"].Value!);
            continue;
        }

        match = memoryRegex.Match(instruction);
        if (!match.Success)
        {
            throw new Exception($"Invalid line {instruction}");
        }

        var address = long.Parse(match.Groups["address"].Value);
        var number = long.Parse(match.Groups["value"].Value);

        onStore(address, number);
    }
}

static long Part1(string[] instructions)
{
    var memory = new Dictionary<long, long>();
    (long mask0, long mask1) = (0, 0);

    Action<string> onMask = (string mask) =>
    {
        mask1 = 0L;
        mask0 = ~0L;

        for (int i = 35; i >= 0; i--)
        {
            if (mask[mask.Length - i - 1] == '0')
            {
                mask0 ^= 1L << i;
            }
            else if (mask[mask.Length - i - 1] == '1')
            {
                mask1 |= 1L << i;
            }
        }
    };

    Action<long, long> onStore = (long address, long value) =>
    {
        value |= mask1;
        value &= mask0;

        memory[address] = value;
    };

    ProcessInput(instructions, onMask, onStore);

    return memory.Values.Sum();
}

Console.WriteLine($"Part 1: {Part1(lines)}");
