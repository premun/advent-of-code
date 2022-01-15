using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

var fields = lines
    .TakeWhile(l => l != "your ticket:")
    .Select(line =>
    {
        var parts = line.Split(':');
        var intervals = parts[1].Trim().Split(" or ");

        return (Name: parts[0], Intervals: intervals.Select(i =>
        {
            var numbers = i.Split('-').Select(int.Parse);
            return (Min: numbers.ElementAt(0), Max: numbers.ElementAt(1));
        }).ToArray());
    })
    .ToArray();

var myTicket = lines
    .SkipWhile(l => l != "your ticket:")
    .ElementAt(1)
    .SplitToNumbers();

var nearbyTickets = lines
    .SkipWhile(l => l != "nearby tickets:")
    .Skip(1)
    .Select(l => l.SplitToNumbers())
    .ToArray();

var invalidRate = nearbyTickets
    .SelectMany(n => n)
    .Where(n => fields.All(f => f.Intervals.All(i => n < i.Min || n > i.Max)))
    .Sum();

Console.WriteLine($"Part 1: {invalidRate}");
