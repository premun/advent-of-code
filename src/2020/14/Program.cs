﻿using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

static void ProcessInput(string[] instructions, Action<string> onMask, Action<long, long> onInstruction)
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

        onInstruction(address, number);
    }
}

static long Part1(string[] instructions)
{
    var memory = new Dictionary<long, long>();
    (long mask0, long mask1) = (0, 0);

    void onMask(string mask)
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
    }

    void onInstruction(long address, long value)
    {
        value |= mask1;
        value &= mask0;

        memory[address] = value;
    }

    ProcessInput(instructions, onMask, onInstruction);

    return memory.Values.Sum();
}

static long Part2(string[] instructions)
{
    var memory = new Dictionary<long, long>();

    long mask1 = 0L;
    long clearMask = 0L;
    var floatingMasks = new List<long>();

    void onMask(string mask)
    {
        mask1 = 0L;
        clearMask = 0L;

        floatingMasks.Clear();

        var positions = new List<int>();

        for (int i = 35; i >= 0; i--)
        {
            if (mask[mask.Length - i - 1] == 'X')
            {
                positions.Add(i);
                clearMask |= 1L << i;
            }
            else if (mask[mask.Length - i - 1] == '1')
            {
                mask1 |= 1L << i;
            }
        }

        clearMask = ~clearMask;

        // Generating number from 0..63 will give us all combinations of 0s and 1s
        // We then put the 1s on the right positions that we noted down above
        for (int i = 0; i < (int)Math.Pow(2, positions.Count); i++)
        {
            var floatingMask = 0L;
            for (int j = 0; j < positions.Count; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    floatingMask |= 1L << positions[j];
                }
            }

            floatingMasks.Add(floatingMask);
        }
    }

    void onStore(long address, long value)
    {
        // 1) Clear out all bits we will be overriding
        address &= clearMask;

        // 2) Add non-floating 1s
        address |= mask1;

        // 3) for all floating combinations, add 1s
        foreach (var floatingMask in floatingMasks)
        {
            memory[address | floatingMask] = value;
        }
    }

    ProcessInput(instructions, onMask, onStore);

    return memory.Values.Sum();
}

Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 2: {Part2(lines)}");
