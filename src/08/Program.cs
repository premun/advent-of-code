using _08;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

static int Part1(IEnumerable<string> lines)
{
    var onesFoursSevensEights = 0;

    foreach (var line in lines)
    {
        var parts = line.SplitBy("|").ToArray();

        onesFoursSevensEights += parts[1]
            .SplitBy(" ")
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
        var digits = line.SplitBy("|").Select(x => x.SplitBy(" ")).ToArray();

        var segmentMapper = new SegmentMapper();
        segmentMapper.ResolveDigits(digits[0]);
        result += segmentMapper.MapDigits(digits[1]);
    }

    return result;
}

Console.WriteLine($"Part 2: {Part2(lines)}");
