using _08;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

static int Part1(IEnumerable<string> lines)
{
    return lines.Select(line => line.SplitBy("|").Last().SplitBy(" ").Count(p => p.Length is 2 or 3 or 4 or 7)).Sum();
}

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

Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 2: {Part2(lines)}");
