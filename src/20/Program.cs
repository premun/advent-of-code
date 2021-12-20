using _20;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");
var enhancementAlgorithm = Array.AsReadOnly(lines.First().Select(c => c == '#').ToArray());

var image = lines.Skip(1).Select(line => line.Select(c => c == '#').ToArray()).ToArray();

var enhancer = new ImageEnhancer(enhancementAlgorithm);

for (int i = 1; i <= 50; i++)
{
    image = enhancer.Enhance(image);

    if (i == 2)
    {
        Console.WriteLine($"Part 1: {image.SelectMany(row => row).Count(pixel => pixel)}");
    }
}

Console.WriteLine($"Part 2: {image.SelectMany(row => row).Count(pixel => pixel)}");

Console.WriteLine();

ImageEnhancer.Display(image);
