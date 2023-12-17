using AdventOfCode.Common;
using Common;
using GetVectors = System.Func<int, int, int, (System.Collections.Generic.IEnumerable<bool>, System.Collections.Generic.IEnumerable<bool>)>;

var patterns = Resources.GetInputFileContent()
    .SplitBy(Environment.NewLine + Environment.NewLine)
    .Select(pattern => pattern.SplitBy(Environment.NewLine))
    .Select(pattern => pattern.ParseAsArray(c => c == '#'))
    .ToList();

Console.WriteLine($"Part 1: {FindReflections(patterns, numOfSmudges: 0)}");
Console.WriteLine($"Part 2: {FindReflections(patterns, numOfSmudges: 1)}");

static int FindReflections(List<bool[,]> patterns, int numOfSmudges)
{
    var result = 0;
    foreach (var pattern in patterns)
    {
        var height = pattern.Height();
        var width = pattern.Width();

        var reflectedRow = GetReflectionPoint(width, height, numOfSmudges, (int row, int col, int size) =>
            (Enumerable.Range(0, size).Select(i => pattern[row + i, col]),
             Enumerable.Range(1, size).Select(i => pattern[row - i, col])));

        if (reflectedRow.HasValue)
        {
            result += 100 * reflectedRow.Value;
        }
        else
        {
            var reflectedCol = GetReflectionPoint(height, width, numOfSmudges, (int col, int row, int size) =>
                (Enumerable.Range(0, size).Select(i => pattern[row, col + i]),
                 Enumerable.Range(1, size).Select(i => pattern[row, col - i])));

            result += reflectedCol ?? throw new Exception("No reflection found for pattern");
        }
    }

    return result;
}

static int? GetReflectionPoint(int width, int height, int numOfSmudges, GetVectors getReflections)
{
    for (int row = 1; row < height; row++)
    {
        var differences = Enumerable.Range(0, width)
            .Sum(col =>
            {
                var size = Math.Min(row, height - row);
                var (a, b) = getReflections(row, col, size);
                return GetDifferences(a, b);
            });

        if (differences == numOfSmudges)
        {
            return row;
        }
    }

    return null;
}

static int GetDifferences(IEnumerable<bool> a, IEnumerable<bool> b)
    => a.Zip(b).Count(p => p.First != p.Second);
