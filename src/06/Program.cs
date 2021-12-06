using _03;
using Common;

var population = Resources.GetResourceFileLines("input.txt")
    .First()
    .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Select(int.Parse)
    .Select(t => new Lanternfish(t));

static int Part1(IEnumerable<Lanternfish> population, int generations)
{
    var thisGeneration = new Queue<Lanternfish>(population);
    var nextGeneration = new Queue<Lanternfish>();

    for (int i = 0; i < generations; i++)
    {
        while (thisGeneration.Count > 0)
        {
            var fish = thisGeneration.Dequeue();

            nextGeneration.Enqueue(fish);

            if (fish.Tick())
            {
                nextGeneration.Enqueue(new Lanternfish(8));
            }
        }

        (thisGeneration, nextGeneration) = (nextGeneration, thisGeneration);
    }

    return thisGeneration.Count();
}

Console.WriteLine($"Part 1: {Part1(population, 80)}");
Console.WriteLine($"Part 1: {Part1(population, 256)}");
