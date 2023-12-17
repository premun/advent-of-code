using System.Text;
using AdventOfCode.Common;

namespace Common;
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
}
