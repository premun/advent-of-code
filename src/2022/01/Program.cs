﻿using AdventOfCode.Common;

var lines = Resources.GetInputFileContent().Split(Environment.NewLine);

var elfs = new List<List<int>>
{
    new()
};

foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        elfs.Add([]);
        continue;
    }

    elfs[^1].Add(int.Parse(line));
}

var orderedElfs = elfs.Select(elf => elf.Sum()).OrderDescending().ToList();

Console.WriteLine($"Part 1: {orderedElfs.First()}");
Console.WriteLine($"Part 2: {orderedElfs.Take(3).Sum()}");
