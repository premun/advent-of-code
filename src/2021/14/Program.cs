using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

string template = lines.First();
Dictionary<(char, char), char> rules = lines
    .Skip(1)
    .Select(line => line.Split(" -> "))
    .ToDictionary(l => (l[0][0], l[0][1]), l => l[1][0]);

static void AddOrBump<TKey>(Dictionary<TKey, long> dict, TKey key, long amount) where TKey : notnull
{
    if (dict.ContainsKey(key))
    {
        dict[key] += amount;
    }
    else
    {
        dict.Add(key, amount);
    }
}

static long[] GetFrequencies(Dictionary<(char, char), long> polymers)
{
    var frequencies = new long['Z' - 'A' + 1];

    foreach (var pair in polymers)
    {
        var polymer1 = pair.Key.Item1;
        var polymer2 = pair.Key.Item2;

        frequencies[polymer1 - 'A'] += pair.Value;
        frequencies[polymer2 - 'A'] += pair.Value;
    }

    return frequencies;
}

void Evolve(Dictionary<(char, char), long> polymers)
{
    foreach (var pair in polymers.ToList())
    {
        var key = pair.Key;
        var count = pair.Value;

        if (count == 0)
        {
            continue;
        }

        if (!rules.ContainsKey(key))
        {
            continue;
        }

        polymers[key] -= count;

        var newPolymer = rules[key];
        AddOrBump(polymers, (key.Item1, newPolymer), count);
        AddOrBump(polymers, (newPolymer, key.Item2), count);
    }
}

long EvolveAndCount(int steps)
{
    // Read
    var polymers = new Dictionary<(char, char), long>();
    for (int i = 1; i < template.Length; i++)
    {
        AddOrBump(polymers, (template[i - 1], template[i]), 1);
    }

    // Evolve
    for (int i = 0; i < steps; i++)
    {
        Evolve(polymers);
    }

    // Count
    var frequencies = GetFrequencies(polymers);

    // Edges are accounted for just once, the rest twice because of overlaps
    // e.g. ABCD turns into (A,B) (B,C) (C,D)
    frequencies[template.First() - 'A']++;
    frequencies[template.Last() - 'A']++;

    return (frequencies.Max() - frequencies.Where(f => f > 0).Min()) / 2;
}

Console.WriteLine($"Part 1: {EvolveAndCount(10)}");
Console.WriteLine($"Part 2: {EvolveAndCount(40)}");
