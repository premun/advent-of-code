using System.Text;

namespace AdventOfCode.Common;

public static class MultiDimensionalArrayExtensions
{
    public static int Height<T>(this T[,] array) => array.GetLength(0);
    public static int Width<T>(this T[,] array) => array.GetLength(1);

    public static T[,] InitializeWith<T>(this T[,] array, T value)
    {
        var h = array.Height();
        var w = array.Width();
        for (int i = 0; i < h * w; i++)
        {
            array[i % h, i / h] = value;
        }

        return array;
    }

    public static T[,] InitializeWith<T>(this T[,] array, Func<int, int, T> filler)
    {
        var h = array.Height();
        var w = array.Width();
        for (int i = 0; i < h * w; i++)
        {
            var row = i % h;
            var col = i / h;
            array[row, col] = filler(row, col);
        }

        return array;
    }

    public static IEnumerable<Coor<int>> AllCoordinates<T>(this T[,] array)
    {
        for (int i = 0; i < array.Height() * array.Width(); i++)
        {
            yield return new Coor<int>(i % array.Height(), i / array.Height());
        }
    }

    public static void ForEach<T>(this T[,] array, Action<int, int, T> action)
    {
        for (int row = 0; row < array.Height(); row++)
            for (int col = 0; col < array.Width(); col++)
                action(row, col, array[row, col]);
    }

    public static string ToFlatString(this char[,] array)
    {
        var h = array.Height();
        var w = array.Width();
        var result = new StringBuilder(h * w);
        for (int i = 0; i < h * w; i++)
        {
            result.Append(array[i % h, i / h]);
        }

        return result.ToString();
    }

    public static void Print(this char[,] map, Func<Coor<int>, char?>? printOverride = null)
    {
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                var c = printOverride?.Invoke(new(y, x)) ?? map[y, x];
                Console.Write(c);
            }

            Console.WriteLine();
        }
    }

    public static void Print<T>(this T[,] map, Func<Coor<int>, char> printOverride)
    {
        for (var y = 0; y < map.Height(); y++)
        {
            for (var x = 0; x < map.Width(); x++)
            {
                Console.Write(printOverride.Invoke(new(y, x)));
            }

            Console.WriteLine();
        }
    }

    // Returns a set of vectors of possible ways to traverse a rectangle:
    // - Rows
    // - Columns
    // - Diagonals
    public static IEnumerable<List<Coor<int>>> GetVectors<T>(this T[,] map)
    {
        var width = map.Width();
        var height = map.Height();

        // Horizontal rows
        for (int row = 0; row < width; row++)
            yield return [.. Enumerable.Range(0, width).Select(col => new Coor<int>(row, col))];

        // Vertical cols
        for (int col = 0; col < width; col++)
            yield return [.. Enumerable.Range(0, height).Select(row => new Coor<int>(row, col))];

        for (int i = 0; i < height + width - 1; i++)
        {
            var diagonal1 = new List<Coor<int>>();
            var diagonal2 = new List<Coor<int>>();

            int row = Math.Min(i, height - 1);
            int col = Math.Max(0, i - height + 1);
            var c1 = new Coor<int>(row, col);
            var c2 = new Coor<int>(row, width - col - 1);

            do
            {
                diagonal1.Add(c1);
                diagonal2.Add(c2);
                row--;
                col++;
                c1 = new Coor<int>(row, col);
                c2 = new Coor<int>(row, width - col - 1);
            }
            while (c1.InBoundsOf(map));

            yield return diagonal1;
            yield return diagonal2;
        }
    }

    public static T Get<T>(this T[,] items, Coor<int> coor)
        => items[coor.Y, coor.X];

    public static T Set<T>(this T[,] items, Coor<int> coor, T value)
        => items[coor.Y, coor.X] = value;
}
