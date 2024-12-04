using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines().ParseAsArray();

// Take all possible vectors across the map (rows, cols, diagonals)
int part1 = map.GetVectors()
    .Where(vector => vector.Count >= 4)
    .SelectMany(vector => Enumerable.Range(0, vector.Count - 4).Select(i => vector.Skip(i).Take(4).ToList()))
    .Count(vector => vector.VectorContainsWord(map, "XMAS") || vector.VectorContainsWord(map, "SAMX"));

// Get all X constelations of coordinates across the map
int part2 = map.GetXMasks()
    .Count(MaskContainsMasMas);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

bool MaskContainsMasMas(IList<Coor> mask)
{
    if (map.Get(mask[2]) != 'A') return false;

    var lt = map.Get(mask[0]);
    var rt = map.Get(mask[1]);
    var lb = map.Get(mask[3]);
    var rb = map.Get(mask[4]);

    return ((lt == 'M' && rb == 'S') || (lt == 'S' && rb == 'M'))
        && ((rt == 'M' && lb == 'S') || (rt == 'S' && lb == 'M'));
}

static file class Extensions
{
    public static bool VectorContainsWord(this IList<Coor> vector, char[,] map, string word)
    {
        if (vector.Count < word.Length) return false;
        if (vector.Where((c, i) => map.Get(vector[i]) != word[i]).Any()) return false;
        return true;
    }

    // Returns sets of coordinates in the shape of an X
    public static IEnumerable<List<Coor<int>>> GetXMasks(this char[,] map)
    {
        var width = map.Width();
        var height = map.Height();

        for (int row = 0; row < height - 2; ++row)
            for (int col = 0; col < width - 2; ++col)
                yield return
                [
                    new Coor<int>(row, col),
                    new Coor<int>(row, col + 2),
                    new Coor<int>(row + 1, col + 1),
                    new Coor<int>(row + 2, col),
                    new Coor<int>(row + 2, col + 2),
                ];
    }
}
