using _08;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

static int Part1(IEnumerable<string> lines)
{
    var onesFoursSevensEights = 0;

    foreach (var line in lines)
    {
        var parts = line.Split("|").Select(x => x.Trim()).ToArray();

        onesFoursSevensEights += parts[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Count(p => p.Length == 2 || p.Length == 3 || p.Length == 4 || p.Length == 7);
    }

    return onesFoursSevensEights;
}

Console.WriteLine($"Part 1: {Part1(lines)}");

static long Part2(IEnumerable<string> lines)
{
    long result = 0;
    foreach (var line in lines)
    {
        var segmentMapper = new SegmentMapper();

        var digits = line.Split("|")
            .Select(x => x.Trim())
            .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToArray();

        foreach (var digit in digits[0])
        {
            segmentMapper.TeachDigit(digit);
        }

        foreach (var digit in digits[1])
        {
            result += segmentMapper.MapDigit(digits[1]);
        }
    }

    return result;
}

Console.WriteLine($"Part 2: {Part2(lines)}");
