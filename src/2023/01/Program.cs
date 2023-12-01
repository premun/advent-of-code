using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 2: {Part1(lines.Select(ReplaceDigits))}");

static int Part1(IEnumerable<string> lines) => lines
    .Select(l => l.Where(char.IsDigit))
    .Select(digits => 10 * digits.First() + digits.Last())
    .Sum();

static string ReplaceDigits(string line)
{
    string[] digits = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

    for (int index = 0; index < line.Length; index++)
    {
        for (int d = 0; d < digits.Length; d++)
        {
            if (line[index..].StartsWith(digits[d]))
            {
                line =
                    line.Substring(0, index + 1)
                    + d
                    + (index < line.Length - 1 ? line.Substring(index + 1) : null);

                index += digits[d].Length - 1;
            }
        }
    }

    return line;
}
