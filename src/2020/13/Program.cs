using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

var arrival = int.Parse(lines.First());
var buses = lines.Last()
    .SplitBy(",")
    .Where(b => b != "x")
    .Select(int.Parse)
    .ToList();

var wait = -1;
int bus = 0;
do
{
    ++wait;
    bus = buses.FirstOrDefault(b => (wait + arrival) % b == 0);
} while (bus == 0);

Console.WriteLine($"Part 1: {bus * wait}");
