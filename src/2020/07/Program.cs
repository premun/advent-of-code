using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var regex = new Regex(
    @"^(?<parent>[a-z ]+) bags contain (?<count1>\d+) (?<child1>[a-z ]+) bags?"
    + @"(, (?<count2>\d+) (?<child2>[a-z ]+) bags?)?"
    + @"(, (?<count3>\d+) (?<child3>[a-z ]+) bags?)?"
    + @"(, (?<count4>\d+) (?<child4>[a-z ]+) bags?)?.$");

var regex2 = new Regex(@"^(?<parent>[a-z ]+) bags contain no other bags.");

var bags = new Dictionary<string, (List<string> Parents, List<(string Child, int Count)> Children)>();

foreach (var line in lines)
{
    var match = regex.Match(line);

    if (!match.Success)
    {
        match = regex2.Match(line);

        if (!match.Success)
        {
            throw new Exception($"Invalid line '{line}'");
        }
    }

    var parent = match.Groups["parent"].Value;

    List<(string, int)> children;
    if (!bags.TryGetValue(parent, out var refs))
    {
        bags[parent] = (_, children) = (new List<string>(), new());
    }
    else
    {
        (_, children) = refs;
    }

    for (int i = 1; i <= 4; i++)
    {
        var child = match.Groups["child" + i].Value;
        if (string.IsNullOrEmpty(child))
        {
            break;
        }

        var count = int.Parse(match.Groups["count" + i].Value);

        List<string> parents;
        if (!bags.TryGetValue(child, out var references))
        {
            bags[child] = (parents, _) = (new(), new List<(string, int)>());
        }
        else
        {
            (parents, _) = references;
        }

        parents.Add(parent);
        children.Add((child, count));
    }
}

IEnumerable<string> FindBagsThatCanHoldABag(string bag)
{
    return bags[bag].Parents.Union(bags[bag].Parents.SelectMany(parent => FindBagsThatCanHoldABag(parent)));
}

int CountBagsInside(string bag)
{
    return bags[bag].Children.Select(c => c.Count * CountBagsInside(c.Child)).Sum() + 1;
}

Console.WriteLine($"Part 1: {FindBagsThatCanHoldABag("shiny gold").Count()}");
Console.WriteLine($"Part 2: {CountBagsInside("shiny gold") - 1}");
