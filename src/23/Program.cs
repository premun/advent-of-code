using _23;
using Common;

var chars = Resources.GetResourceFile("input.txt")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Select(field => field).ToArray())
    .ToArray();

var world = new AmphipodWorld(chars);

Console.WriteLine(world);

//Console.WriteLine($"Part 1: ");
