using System.Text.RegularExpressions;
using AdventOfCode.Common;

var content = Resources.GetInputFileContent("input.txt");
var mulRegex = new Regex(@"(?<instruction>mul\((?<number1>\d+),(?<number2>\d+)\)|do\(\)|don't\(\))");

var disabled = false;
uint sum = 0;
uint enabledOnlySum = 0;
foreach (Match match in mulRegex.Matches(content))
{
    var instruction = match.Groups["instruction"].Value;

    switch (instruction)
    {
        case "do()":
            disabled = false;
            continue;
        case "don't()":
            disabled = true;
            continue;
        default:
            var number1 = uint.Parse(match.Groups["number1"].Value);
            var number2 = uint.Parse(match.Groups["number2"].Value);
            var mul = number1 * number2;
            sum += mul;
            if (!disabled)
            {
                enabledOnlySum += mul;
            }

            continue;
    }
}

Console.WriteLine($"Part 1: {sum}");
Console.WriteLine($"Part 2: {enabledOnlySum}");
