using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileContent().Split(Environment.NewLine);

var stackSetup = lines.TakeWhile(l => !string.IsNullOrEmpty(l)).Reverse().ToList();
var stackCount = stackSetup.First().Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

var instructionRegex = new Regex(@"^move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)$");
var instructions = lines
    .SkipWhile(l => !string.IsNullOrEmpty(l))
    .Where(l => !string.IsNullOrEmpty(l))
    .Select(l => instructionRegex.Match(l))
    .Select(match => new Instruction(
        int.Parse(match.Groups["count"].Value),
        int.Parse(match.Groups["from"].Value) - 1,
        int.Parse(match.Groups["to"].Value) - 1))
    .ToList();

static Stack<char>[] GetInitialStackState(IEnumerable<string> setup, int stackCount)
{
    var stacks = Enumerable.Range(1, stackCount).Select(_ => new Stack<char>()).ToArray();
    var crateRegex = new Regex(@"^((\[([A-Z])\]|   ) ?){0," + stackCount + "}$");

    foreach (var line in setup.Skip(1))
    {
        var match = crateRegex.Match(line);
        for (int i = 0; i < stackCount; i++)
        {
            var crate = match.Groups[1].Captures[i].Value[1];
            if (crate != ' ')
            {
                stacks[i].Push(crate);
            }
        }
    }

    return stacks;
}

var stacks1 = GetInitialStackState(stackSetup, stackCount);
foreach (var instruction in instructions)
{
    for (int i = 0; i < instruction.Count; i++)
    {
        stacks1[instruction.To].Push(stacks1[instruction.From].Pop());
    }
}

var stacks2 = GetInitialStackState(stackSetup, stackCount);
foreach (var instruction in instructions)
{
    var tmpStack = new Stack<char>();
    for (int i = 0; i < instruction.Count; i++)
    {
        tmpStack.Push(stacks2[instruction.From].Pop());
    }

    for (int i = 0; i < instruction.Count; i++)
    {
        stacks2[instruction.To].Push(tmpStack.Pop());
    }
}

Console.WriteLine($"Part 1: {new string(stacks1.Select(s => s.Peek()).ToArray())}");
Console.WriteLine($"Part 2: {new string(stacks2.Select(s => s.Peek()).ToArray())}");

file record Instruction(int Count, int From, int To);
