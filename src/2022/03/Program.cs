using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

static int GetScore(char c) => c + 1 - (c >= 'a' ? 'a' : 'A' - 26);

var itemsInBothCompartments = lines
    .Select(line => (Left: line.Substring(0, line.Length / 2), Right: line.Substring(line.Length / 2)))
    .SelectMany(rucksack => rucksack.Left.Where(c => rucksack.Right.Contains(c)).Distinct());

var badges = lines.GroupsOf(3)
    .Select(group => group.Select(elf => elf.Distinct()))
    .Select(group => group.First().First(item => group.All(items => items.Contains(item))));

Console.WriteLine($"Part 1: {itemsInBothCompartments.Select(GetScore).Sum()}");
Console.WriteLine($"Part 2: {badges.Select(GetScore).Sum()}");
