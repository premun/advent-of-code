using Common;

var lines = Resources.GetResourceFileLines("input.txt");

var position = (0, 0);

// Part 1
foreach (var line in lines)
{
    var parts = line.Split(' ');
    var direction = parts[0];
    var distance = int.Parse(parts[1]);

    switch (direction)
    {
        case "forward":
            position.Item2 += distance;
            break;

        case "down":
            position.Item1 += distance;
            break;

        case "up":
            position.Item1 -= distance;
            break;
    }
}

Console.WriteLine($"Part 1: {position.Item1 * position.Item2}");

// Part 2
position = (0, 0);
var aim = 0;

foreach (var line in lines)
{
    var parts = line.Split(' ');
    var direction = parts[0];
    var distance = int.Parse(parts[1]);

    switch (direction)
    {
        case "forward":
            position.Item1 += aim * distance;
            position.Item2 += distance;
            break;

        case "down":
            aim += distance;
            break;

        case "up":
            aim -= distance;
            break;
    }
}

Console.WriteLine($"Part 2: {position.Item1 * position.Item2}");
