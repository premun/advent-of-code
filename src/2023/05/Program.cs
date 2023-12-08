using System.Text.RegularExpressions;
using AdventOfCode.Common;
using Range = AdventOfCode.Common.Range;

var lines = Resources.GetInputFileLines();
var headerPattern = new Regex(@"(?<from>[a-z]+)\-to\-(?<to>[a-z]+) map:");

var seeds = lines.First().ParseLongNumbersOut();
var categories = new Dictionary<string, Category>();

Category category = null!;
foreach (var line in lines.Skip(1))
{
    var match = headerPattern.Match(line);
    if (match.Success)
    {
        if (category != null)
        {
            categories.Add(category.From, category);
        }

        var to = match.Groups["to"].Value;

        category = new Category(
            match.Groups["from"].Value,
            to == "location" ? null : to,
            []);
        continue;
    }

    if (line.ParseLongNumbersOut() is [long dest, long source, long length])
    {
        category.Rules.Add(new(dest, source, length));
    }
}

categories.Add(category.From, category);

// Quite ashamed of this but it did finish in a couple of minutes
var part2 = seeds
    .GroupsOf(2)
    .Select(pair => new Range(pair.First(), pair.First() + pair.Last() - 1))
    .Select(range => range.Enumerate().AsParallel().Select(i => GetLocation(i)).Min())
    .Min();

Console.WriteLine($"Part 1: {seeds.Select(s => GetLocation(s)).Min()}");
Console.WriteLine($"Part 2: {part2}");

long GetLocation(long seed, string categoryName = "seed")
{
    var category = categories[categoryName];
    var mappedValue = category.Map(seed);

    return category.To is null
        ? mappedValue
        : GetLocation(mappedValue, category.To);
}

file record Rule : Range
{
    public Rule(long destination, long source, long rangeLength)
        : base(source, source + rangeLength - 1)
    {
        Destination = destination;
    }

    public long Destination { get; }

    public long? Map(long number) => Contains(number) ? number + Destination - Start : null;
}

file record Category(string From, string? To, IList<Rule> Rules)
{
    public long Map(long number)
        => Rules.FirstOrDefault(r => r.Map(number) != null)?.Map(number) ?? number;
}
