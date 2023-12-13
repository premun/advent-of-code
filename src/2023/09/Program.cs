using AdventOfCode.Common;

var extrapolatedNumbers = Resources.GetInputFileLines()
    .Select(Resources.ParseLongNumbersOut)
    .Select(Extrapolate)
    .Aggregate((Start: 0L, End: 0L), (acc, v) => (acc.Start + v.Start, acc.End + v.End));

Console.WriteLine($"Part 1: {extrapolatedNumbers.End}");
Console.WriteLine($"Part 2: {extrapolatedNumbers.Start}");

static (long Start, long End) Extrapolate(List<long> numbers)
{
    var sequences = new List<List<long>>(numbers.Count - 1)
    {
        numbers
    };

    bool allZeros = false;
    do
    {
        allZeros = true;
        var last = sequences.Last();
        if (last.Count == 1)
        {
            break;
        }

        var bottom = new List<long>(last.Count - 1);
        for (int i = 1; i < last.Count; i++)
        {
            var n = last[i] - last[i - 1];
            bottom.Add(n);
            allZeros &= n == 0;
        }

        sequences.Add(bottom);
    } while (!allZeros);

    var start = sequences.Last().First();
    var end = sequences.Last().Last();
    for (int i = sequences.Count - 1; i >= 0; --i)
    {
        end = sequences[i].Last() + end;
        start = sequences[i].First() - start;
    }

    return (start, end);
}
