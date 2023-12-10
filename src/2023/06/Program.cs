using AdventOfCode.Common;

/*
maxTime = 15
distance > 40
distance = (maxTime - wait) * wait
wait > 0
wait < maxTime


distance = (15 - wait) * wait
0 = -wait^2 + maxTime * wait - distance

a = -1
b = m
c = -d

x = (-maxTime +/- sqrt(maxTime^2 - 8 * distance)) / -4
*/

var lines = Resources.GetInputFileLines();
List<List<int>> numbers = lines
    .Select(l => l.ParseNumbersOut())
    .ToList();

List<long> unkernedNumbers = lines
    .Select(s => s.Replace(" ", null))
    .Select(s => s.ParseLongNumbersOut().First())
    .ToList();

long part1 = numbers
    .First()
    .Zip(numbers.Last())
    .Select(p => GetNumberOfWays(p.First, p.Second))
    .Multiply();

long part2 = GetNumberOfWays(unkernedNumbers.First(), unkernedNumbers.Last());

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static long GetNumberOfWays(long maxTime, long maxDistance)
{
    var (min, max) = Solve(maxTime, maxDistance);
    min = Math.Max(1, min);
    max = Math.Min(max, maxTime - 1);

    var ways = (int)(Math.Floor(max) - Math.Ceiling(min)) + 1;

    // Correction for boundaries
    if (GetDistance(maxTime, (int)Math.Ceiling(min)) <= maxDistance)
    {
        ways--;
    }

    if (GetDistance(maxTime, (int)Math.Floor(max)) <= maxDistance)
    {
        ways--;
    }

    return ways;
}

static long GetDistance(long maxTime, long waitTime) => (maxTime - waitTime) * waitTime;

static (double, double) Solve(long maxTime, long maxDistance)
{
    var root = Math.Sqrt(maxTime * maxTime - 4 * maxDistance);
    double Calc(double r) => (- maxTime + r) / -2;
    var (min, max) = (Calc(root), Calc(-root));
    return min < max ? (min, max) : (max, min);
}
