using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var trees = Resources.GetInputFileLines().ParseAsArray(c => c - '0');

var topDown = Enumerable.Range(0, trees.GetLength(0)).ToArray();
var leftRight = Enumerable.Range(0, trees.GetLength(1)).ToArray();

var treeLines =
    topDown.Select(row => leftRight.Select(col => (row, col)))                     // Rows, left to right
    .Concat(topDown.Select(row => leftRight.Reverse().Select(col => (row, col))))  // Rows, right to left
    .Concat(leftRight.Select(col => topDown.Select(row => (row, col))))            // Columns, top to bottom
    .Concat(leftRight.Select(col => topDown.Reverse().Select(row => (row, col)))); // Columns, bottom to top

var visibleTrees = treeLines.SelectMany(line => line.AllVisible(trees));

var scenicScores =
    from row in topDown.Skip(1).Take(topDown.Length - 2)
    from col in leftRight.Skip(1).Take(leftRight.Length - 2)
    select Coor.FourWayNeighbours.Select(dir =>
    {
        var coor = new Coor(row, col);
        var treesVisible = 0;

        do
        {
            coor += dir;

            if (!coor.InBoundsOf(trees))
            {
                break;
            }
            
            ++treesVisible;

        } while (trees[coor.Y, coor.X] < trees[row, col]);

        return treesVisible;
    }).Aggregate(1, (acc, x) => acc * x);

Console.WriteLine($"Part 1: {visibleTrees.Distinct().Count()}");
Console.WriteLine($"Part 2: {scenicScores.Max()}");

static file class Trees
{
    public static IEnumerable<(int, int)> AllVisible(this IEnumerable<(int, int)> treeLine, int[,] trees)
    {
        return treeLine.Where((_, index) => treeLine.IsVisible(index, trees));
    }

    private static bool IsVisible(this IEnumerable<(int Row, int Col)> treeLine, int index, int[,] trees)
    {
        if (index == 0)
        {
            return true;
        }

        var tree = treeLine.ElementAt(index);
        return trees[tree.Row, tree.Col] > treeLine.Take(index).Select(t => trees[t.Row, t.Col]).Max();
    }
}
