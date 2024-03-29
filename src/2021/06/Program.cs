﻿using AdventOfCode.Common;

var population = Resources.GetInputFileLines().First().SplitToNumbers();

static long[] CreateHistogram(IEnumerable<int> population)
{
    var histogram = new long[9];

    foreach (var i in population)
    {
        histogram[i]++;
    }

    return histogram;
}

static long Evolve(IEnumerable<int> population, int generations)
{
    long[] histogram = CreateHistogram(population);

    for (int i = 0; i < generations; i++)
    {
        var readyToBreed = histogram[0];
        for (int j = 0; j < histogram.Length - 1; j++)
        {
            histogram[j] = histogram[j + 1];
        }

        histogram[8] = readyToBreed;
        histogram[6] += readyToBreed;
    }

    return histogram.Sum();
}

Console.WriteLine($"Part 1: {Evolve(population, 80)}");
Console.WriteLine($"Part 2: {Evolve(population, 256)}");
