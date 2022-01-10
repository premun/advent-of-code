using AdventOfCode.Common;

var numbers = Resources.GetResourceFile("input.txt")
    .Split(Environment.NewLine)
    .Where(s => !string.IsNullOrEmpty(s))
    .Select(x => long.Parse(x));

// Part 1
var part1 = numbers.First(n => numbers.Any(m => m + n == 2020));
Console.WriteLine(part1 * (2020 - part1));

// Part 2
foreach (var n in numbers)
{
    foreach (var m in numbers)
    {
        foreach (var o in numbers)
        {
            if (n + m + o == 2020)
            {
                Console.WriteLine(n * m * o);
                return;
            }
        }
    }
}
