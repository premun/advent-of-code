using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var regex = new Regex(@"(\d+)-(\d+),(\d+)-(\d+)");

var assignments = lines
    .Select(line => regex.Match(line).Groups)
    .Select(g =>
        (new Assignment(int.Parse(g[1].Value), int.Parse(g[2].Value)),
         new Assignment(int.Parse(g[3].Value), int.Parse(g[4].Value))))
    .Select(a => a.Item1.Start <= a.Item2.Start ? a : new Row(a.Item2, a.Item1))
    .ToList();

var inclusiveAssignments = assignments
    .Where(a => (a.Item2.Start <= a.Item1.End && a.Item2.End <= a.Item1.End) || a.Item2.Start == a.Item1.Start);

var overlappingAssignments = assignments
    .Where(a => a.Item2.Start <= a.Item1.End);

Console.WriteLine($"Part 1: {inclusiveAssignments.Count()}");
Console.WriteLine($"Part 2: {overlappingAssignments.Count()}");

file record Assignment(int Start, int End);
