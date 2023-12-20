using System.Data;
using AdventOfCode.Common;
using Range = AdventOfCode.Common.Range;

var lines = Resources.GetInputFileLines();

var workflows = lines
    .Where(l => !l.StartsWith('{'))
    .Select(Workflow.FromDefinition)
    .ToDictionary(w => w.Name);

var parts = lines
    .Where(l => l.StartsWith('{'))
    .Select(Part.FromDefinition)
    .ToList();

//var acceptedParts = parts
//    .Where(IsAccepted)
//    .Select(part => 0L + part.X + part.M + part.A + part.S)
//    .Sum();
//Console.WriteLine($"Part 1: {acceptedParts}");

var paths = FindPaths();

var ranges = new List<(string Type, Range Range)>();


var xRanges = new List<Range?>();
var mRanges = new List<Range?>();
var aRanges = new List<Range?>();
var sRanges = new List<Range?>();

var a = 0L;
foreach (var path in paths)
{
    Console.WriteLine(string.Join(" -> ", path.Select(p => p.Workflow)));
    var rules = path
        .ToLookup(p => p.GetType(), p => p);

    var xRange = new Range(1, 4000);
    var mRange = new Range(1, 4000);
    var aRange = new Range(1, 4000);
    var sRange = new Range(1, 4000);

    foreach (var group in rules)
    {
        foreach (var rule in group)
        {
            switch (group.Key.Name[0])
            {
                case 'X':
                    xRange = xRange?.Intersect(rule.LessThan ? new Range(1, rule.Target) : new Range(rule.Target + 1, int.MaxValue));
                    break;

                case 'M':
                    mRange = mRange?.Intersect(rule.LessThan ? new Range(1, rule.Target) : new Range(rule.Target + 1, int.MaxValue));
                    break;

                case 'A':
                    aRange = aRange?.Intersect(rule.LessThan ? new Range(1, rule.Target) : new Range(rule.Target + 1, int.MaxValue));
                    break;

                case 'S':
                    sRange = sRange?.Intersect(rule.LessThan ? new Range(1, rule.Target) : new Range(rule.Target + 1, int.MaxValue));
                    break;
            };
        }
    }

    Console.WriteLine($"     " + string.Join(" ,", xRange, mRange, aRange, sRange));

    xRanges.Add(xRange);
    mRanges.Add(mRange);
    aRanges.Add(aRange);
    sRanges.Add(sRange);

    a += (xRange?.End ?? 0 - xRange?.Start ?? 0)
        * (mRange?.End ?? 0 - mRange?.Start ?? 0)
        * (aRange?.End ?? 0 - aRange?.Start ?? 0)
        * (sRange?.End ?? 0 - sRange?.Start ?? 0);

    Console.WriteLine();
    Console.WriteLine();
}

var total = 1L;
foreach (var ranges2 in new[] { xRanges, mRanges, aRanges, sRanges })
{
    var count = 0;
    for (int i = 1; i <= 4000; i++)
    {
        if (ranges2.Any(r => r?.Contains(i) ?? false))
        {
            count++;
        }
    }
    Console.WriteLine("!!! " + count);
    total += count;
}

Console.WriteLine(a);
Console.WriteLine(4000L * 4000L * 4000L * 4000L - a);
Console.WriteLine(total);


List<List<Rule>> FindPathsRecursive(string workflow, List<Rule> path)
{
    if (workflow == "A")
    {
        return [[..path]];
    }
    if (workflow == "R")
    {
        return [];
    }

    var result = new List<List<Rule>>();

    foreach (var rule in workflows![workflow].Rules)
    {
        // No loops
        //if (path.Any(r => r. == rule.Workflow))
        //{
        //    continue;
        //}

        result.AddRange(FindPathsRecursive(rule.Workflow, [.. path, rule]));
        path.Add(rule with
        {
            LessThan = !rule.LessThan,
            Target = rule.LessThan ? rule.Target - 1 : rule.Target + 1,
            Workflow = "!" + rule.Workflow,
        });
    }

    return result;
}

List<List<Rule>> FindPaths()
{
    return FindPathsRecursive("in", []);
}

//bool IsAccepted(Part part)
//{
//    var workflow = "in";
//    while (workflow != "A" && workflow != "R")
//    {
//        workflow = workflows[workflow].Evaluate(part);
//    }

//    return workflow == "A";
//}
