using AdventOfCode.Common;

var adapters = Resources.GetResourceFileLines("input.txt")
    .Select(int.Parse)
    .OrderBy(x => x)
    .ToArray();

static (int, int, int, int) Part1(IEnumerable<int> adapters)
{
    var prev = 0;
    var j1 = 0;
    var j2 = 0;
    var j3 = 0;

    foreach (var a in adapters)
    {
        switch (a - prev)
        {
            case 1:
                j1++;
                break;

            case 2:
                j2++;
                break;

            case 3:
                j3++;
                break;

            default:
                return (prev + 3, j1, j2, j3);
        }

        prev = a;
    }

    return (prev + 3, j1, j2, j3 + 1);
}

// Finds different ways we can omit elements of a sequence but keeps continuity (max diff 3)
// Doesn't work well for long sequences but could theoretically process the whole input and return the right result
// Can be used on the example input for instance
// Would work even when sequence would contain 2's too
// Results could be cached but I am lazy
static long Part2Recursive(int[] differences, int currentDifference, int index)
{
    if (index == differences.Length - 1)
    {
        return 1;
    }

    var difference = currentDifference + differences[index];

    if (difference + differences[index + 1] <= 3)
    {
        return Part2Recursive(differences, 0, index + 1) + Part2Recursive(differences, difference, index + 1);
    }
    else
    {
        return Part2Recursive(differences, 0, index + 1);
    }
}

static long Part2(int[] adapters)
{
    // Transforms adapter sequence into list of differences between the numbers
    var differences = adapters
        .Select((x, index) => x - (index == 0 ? 0 : adapters[index - 1]))
        .Select(x => (char)('0' + x))
        .Append('3')
        .ToArray();

    return new string(differences) // Gets the sequence as string of 1111333131111313
        .Split('3')                // Give me chunks separated by 3's because those can be computed separately
        .Select(x => $"3{x}3")     // Wrap them in 3's to make the adapters at the start and end required
        .Select(x => x.Select(y => y - '0').ToArray())
        .Select(x => Part2Recursive(x, 0, 0)) // Compute the number of options we have for every chunk
        .Aggregate((x, acc) => x * acc);      // Multiply those
}

Console.WriteLine($"Part 1: {Part1(adapters)}");
Console.WriteLine($"Part 2: {Part2(adapters)}");
