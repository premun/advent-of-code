using System.Collections.ObjectModel;

namespace _20;

class ImageEnhancer
{
    private static readonly IEnumerable<Coor> _arounds = new Coor[]
    {
        new (-1, -1),
        new (-1, 0),
        new (-1, 1),

        new (0, -1),
        new (0, 0),
        new (0, 1),

        new (1, -1),
        new (1, 0),
        new (1, 1),
    };

    private readonly ReadOnlyCollection<bool> _algorithm;
    private bool _backgroundColor = false;

    public ImageEnhancer(ReadOnlyCollection<bool> algorithm)
    {
        _algorithm = algorithm;
    }

    public bool[][] Enhance(bool[][] image, int boundary = 1)
    {
        var height = image.Length;
        var width = image[0].Length;

        var result = new bool[height + boundary * 2][];

        for (int y = -boundary; y <= height + boundary - 1; y++)
        {
            result[y + boundary] = new bool[width + boundary * 2];

            for (int x = -boundary; x <= width + boundary - 1; x++)
            {
                result[y + boundary][x + boundary] = ResolvePixel(image, y, x);
            }
        }

        // Resolve the infinite surroundings
        // If it is dark and 9 darks turn into a light => flip the background
        // If it is lit and 9 lights turn into dark => also flip
        if ((!_backgroundColor && _algorithm[0]) || (_backgroundColor && _algorithm.Last()))
        {
            _backgroundColor = !_backgroundColor;
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
        var height = image.Length;
        var width = image[0].Length;

        foreach (var c in _arounds)
        {
            value <<= 1;

            var coor = position + c;
            if (coor.Y >= 0 && coor.Y < height && coor.X >= 0 && coor.X < width)
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
                if (_backgroundColor)
                {
                    value |= 1;
                }
            }
        }

        return _algorithm[value];
    }
}
