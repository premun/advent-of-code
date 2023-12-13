using AdventOfCode.Common;

var patterns = Resources.GetInputFileContent()
    .SplitBy(Environment.NewLine + Environment.NewLine)
    .Select(pattern => pattern.SplitBy(Environment.NewLine))
    .Select(pattern => pattern.ParseAsArray(c => c == '#'))
    .ToList();

var result = 0;
foreach (var pattern in patterns)
{
    var height = pattern.GetLength(0);
    var width = pattern.GetLength(1);

    var reflectedRow = GetReflection(width, height, (int row, int col, int size) =>
        (Enumerable.Range(0, size).Select(i => pattern[row + i, col]).BitsToInt(),
         Enumerable.Range(1, size).Select(i => pattern[row - i, col]).BitsToInt()));

    if (reflectedRow.HasValue)
    {
        result += 100 * reflectedRow.Value;
    }
    else
    {
        var reflectedCol = GetReflection(height, width, (int col, int row, int size) =>
            (Enumerable.Range(0, size).Select(i => pattern[row, col + i]).BitsToInt(),
             Enumerable.Range(1, size).Select(i => pattern[row, col - i]).BitsToInt()));

        if (reflectedCol.HasValue)
        {
            result += reflectedCol.Value;
        }
        else
        {
            throw new Exception("No reflection found for pattern");
        }
    }
}

Console.WriteLine($"Part 1: {result}");

static int? GetReflection(int width, int height, Func<int, int, int, (int, int)> getReflections)
{
    for (int dim1 = 1; dim1 < height; dim1++)
    {
        var isReflection = Enumerable.Range(0, width)
            .All(dim2 =>
            {
                var size = Math.Min(dim1, height - dim1);
                var (a, b) = getReflections(dim1, dim2, size);
                return a == b;
            });

        if (isReflection)
        {
            return dim1;
        }
    }

    return null;
}
