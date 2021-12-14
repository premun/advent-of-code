using System.Text;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

string template = lines.First();
Dictionary<string, string> rules = lines.Skip(1).Select(line => line.Split(" -> ")).ToDictionary(l => l[0], l => l[1]);

static string InsertPolymers(string template, Dictionary<string, string> rules)
{
    var result = new StringBuilder();

    for (int i = 1; i < template.Length; i++)
    {
        result.Append(template[i - 1]);

        if (rules.TryGetValue(template[i - 1] + "" + template[i], out var inserted))
        {
            result.Append(inserted);
        }
    }

    result.Append(template.Last());

    return result.ToString();
}

static long Evolve(string template, Dictionary<string, string> rules, int steps)
{
    for (int i = 0; i < steps; i++)
    {
        Console.Write("\r{0}/{1}\t", i + 1, steps);
        template = InsertPolymers(template, rules);
    }

    var frequencies = new Dictionary<char, long>();

    for (int i = 0; i < template.Length; i++)
    {
        if (frequencies.ContainsKey(template[i]))
        {
            frequencies[template[i]]++;
        }
        else
        {
            frequencies.Add(template[i], 1);
        }
    }

    var f = frequencies.Select(p => p.Value);
    return f.Max() - f.Min();
}

Console.WriteLine($"Part 1: {Evolve(template, rules, 10)}");
Console.WriteLine($"Part 2: {Evolve(template, rules, 40)}");
