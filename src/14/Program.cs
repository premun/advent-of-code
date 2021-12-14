using Common;

var lines = Resources.GetResourceFileLines("input.txt");

string template = lines.First();
Dictionary<(char, char), char> rules = lines
    .Skip(1)
    .Select(line => line.Split(" -> "))
    .ToDictionary(l => (l[0][0], l[0][1]), l => l[1][0]);

static void AddOrBump<TKey>(Dictionary<TKey, long> dict, TKey key) where TKey : notnull
{
    if (dict.ContainsKey(key))
    {
        dict[key]++;
    }
    else
    {
        dict.Add(key, 1);
    }
}

static long Evolve(string template, Dictionary<(char, char), char> rules, int steps)
{
    var polymers = new Dictionary<(char, char), long>();

    for (int i = 1; i < template.Length; i++)
    {
        var key = (template[i - 1], template[i]);
        AddOrBump(polymers, key);
    }

    for (int i = 0; i < steps; i++)
    {
        foreach (var pair in polymers.ToList())
        {
            var key = pair.Key;
            var count = pair.Value;

            if (!rules.ContainsKey(key) || count == 0)
            {
                continue;
            }

            polymers[key]--;

            var newPolymer = rules[key];

            AddOrBump(polymers, (key.Item1, newPolymer));
            AddOrBump(polymers, (newPolymer, key.Item2));
        }
    }

    var frequencies = new long['Z' - 'A' + 1];

    frequencies[template.First() - 'A']++;
    frequencies[template.Last() - 'A']++;

    foreach (var pair in polymers)
    {
        var polymer1 = pair.Key.Item1;
        var polymer2 = pair.Key.Item2;

        frequencies[polymer1 - 'A'] += pair.Value;
        frequencies[polymer2 - 'A'] += pair.Value;
    }

    return (frequencies.Max() - frequencies.Min()) / 2;
}

Console.WriteLine($"Part 1: {Evolve(template, rules, 4)}");
// Console.WriteLine($"Part 2: {Evolve(template, rules, 40)}");
