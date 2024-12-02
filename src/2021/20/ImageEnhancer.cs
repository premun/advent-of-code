using System.Collections.ObjectModel;
using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2021_20;

class ImageEnhancer(ReadOnlyCollection<bool> algorithm)
{
    private static readonly IEnumerable<Coor> s_arounds =
    [
        new (-1, -1),
        new (-1, 0),
        new (-1, 1),

        new (0, -1),
        new (0, 0),
        new (0, 1),

        new (1, -1),
        new (1, 0),
        new (1, 1),
    ];

    private readonly ReadOnlyCollection<bool> _algorithm = algorithm;

    // Represents what colour is the inifinite surrounding
    private bool _surroundingColor = false;

    public bool[][] Enhance(bool[][] image)
    {
        var height = image.Length;
        var width = image[0].Length;
        int boundary = 2;

        var result = new bool[height + boundary * 2][];

        for (int y = -boundary; y <= height + boundary - 1; y++)
        {
            result[y + boundary] = new bool[width + boundary * 2];

            for (int x = -boundary; x <= width + boundary - 1; x++)
            {
                result[y + boundary][x + boundary] = ResolvePixel(image, y, x);
            }
        }

        // Check the infinite surroundings
        // If it is dark and 9 darks turn into a light => flip the background
        // If it is lit and 9 lights turn into dark => also flip
        if ((!_surroundingColor && _algorithm[0]) || (_surroundingColor && !_algorithm.Last()))
        {
            _surroundingColor = !_surroundingColor;
        }

        return result;
    }

    public static void Display(bool[][] image)
    {
        for (int y = 0; y < image.Length; y++)
        {
            for (int x = 0; x < image[y].Length; x++)
            {
                Console.Write(image[y][x] ? '#' : '.');
            }

            Console.WriteLine();
        }
    }

    private bool ResolvePixel(bool[][] image, int y, int x)
    {
        var value = 0;
        var position = new Coor(Y: y, X: x);

        foreach (var c in s_arounds)
        {
            value <<= 1;

            var coor = position + c;
            if (coor.Y >= 0 && coor.Y < image.Length && coor.X >= 0 && coor.X < image[0].Length)
            {
                // Within bounds, we look at the input image
                if (image[coor.Y][coor.X])
                {
                    value |= 1;
                }
            }
            else
            {
                // Outside of bounds, we check the fake infinite surroundings
                if (_surroundingColor)
                {
                    value |= 1;
                }
            }
        }

        return _algorithm[value];
    }
}
