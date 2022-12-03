using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

static void Simulate(string[] input, Dictionary<char, Action<int>> actions)
{
    var regex = new Regex(@"^(?<dir>[NSWELRF])(?<value>\d+)$");

    foreach (var line in input)
    {
        var match = regex.Match(line);
        if (!match.Success)
        {
            throw new Exception("??? " + line);
        }

        var direction = match.Groups["dir"].ValueSpan[0];
        var value = match.Groups["value"].ValueSpan;

        actions[direction](int.Parse(value));
    }
}

var direction = 0;
var x = 0;
var y = 0;

var instructions = new Dictionary<char, Action<int>>
{
    { 'N', d => y -= d },
    { 'S', d => y += d },
    { 'W', d => x -= d },
    { 'E', d => x += d },
    { 'R', d => direction = (direction + d) % 360 },
    { 'L', d => direction = (direction - d + 360) % 360 },
    { 'F', d => (x, y) = direction switch
        {
            0 =>   (x + d, y),
            90 =>  (x, y + d),
            180 => (x - d, y),
            270 => (x, y - d),
            _ => throw new Exception("Angle not %90")
        }
    },
};

Simulate(lines, instructions);

Console.WriteLine($"Part 1: {Math.Abs(x) + Math.Abs(y)}");

x = 0;
y = 0;
var wx = 10;
var wy = 1;

instructions = new Dictionary<char, Action<int>>
{
    { 'N', d => wy += d },
    { 'S', d => wy -= d },
    { 'W', d => wx -= d },
    { 'E', d => wx += d },
    { 'R', d => (wx, wy) = d switch
        {
            0 =>   (wx, wy),
            90 =>  (wy, -wx),
            180 => (-wx, -wy),
            270 => (-wy, wx),
            _ => throw new Exception("Angle not %90")
        }
    },
    { 'L', d => (wx, wy) = d switch
        {
            0 =>   (wx, wy),
            90 =>  (-wy, wx),
            180 => (-wx, -wy),
            270 => (wy, -wx),
            _ => throw new Exception("Angle not %90")
        }
    },
    { 'F', d => (x, y) = (x + wx * d, y - wy * d) },
};

Simulate(lines, instructions);

Console.WriteLine($"Part 2: {Math.Abs(x) + Math.Abs(y)}");
