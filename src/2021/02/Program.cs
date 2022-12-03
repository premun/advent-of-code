using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

static void Simulate(string[] lines, Action<int> forward, Action<int> down, Action<int> up)
{
    foreach (var line in lines)
    {
        var parts = line.Split(' ');
        var direction = parts[0];
        var distance = int.Parse(parts[1]);

        switch (direction)
        {
            case "forward":
                forward(distance);
                break;

            case "down":
                down(distance);
                break;

            case "up":
                up(distance);
                break;
        }
    }
}

var position = (0, 0);

// Part 1
Simulate(lines,
    f => position.Item2 += f,
    d => position.Item1 += d,
    u => position.Item1 -= u);

Console.WriteLine($"Part 1: {position.Item1 * position.Item2}");

// Part 2
position = (0, 0);
var aim = 0;

Simulate(lines,
    f =>
    {
        position.Item1 += aim * f;
        position.Item2 += f;
    },
    d => aim += d,
    u => aim -= u);

Console.WriteLine($"Part 2: {position.Item1 * position.Item2}");
