using AdventOfCode._2021_20;
using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");
var enhancementAlgorithm = lines.First().Select(c => c == '#').ToList().AsReadOnly();

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

ImageEnhancer.Display(image);
