using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

static bool IsValidPassword(string definition)
{
    var parts = definition.Split(' ');
    var limits = parts[0].Split('-');
    var min = int.Parse(limits[0]);
    var max = int.Parse(limits[1]);

    var c = parts[1][0];
    var occurences = parts[2].Count(x => c == x);

    return occurences >= min && occurences <= max;
}

static bool IsValidPassword2(string definition)
{
    var parts = definition.Split(' ');
    var limits = parts[0].Split('-');
    var pos1 = int.Parse(limits[0]) - 1;
    var pos2 = int.Parse(limits[1]) - 1;
    var c = parts[1][0];

    return (parts[2][pos1] == c) != (parts[2][pos2] == c);
}

Console.WriteLine($"Part 1: {lines.Count(IsValidPassword)}");
Console.WriteLine($"Part 2: {lines.Count(IsValidPassword2)}");
