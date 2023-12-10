using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();
var instructionCounter = new CircularCounter(lines.First());
var instructionRegex = new Regex("[A-Z0-9]{3}");
var rawInstructions = lines
    .Skip(1)
    .Select(line => instructionRegex.Matches(line))
    .ToDictionary(m => m[0].Value, m => (m[1].Value, m[2].Value));
var instructions = rawInstructions.Keys
    .ToDictionary(name => name, name => new Instruction(name));

foreach (var instruction in instructions)
{
    var i = rawInstructions[instruction.Key];
    instruction.Value.Left = instructions.TryGetValue(i.Item1, out var v1) ? v1 : null!;
    instruction.Value.Right = instructions.TryGetValue(i.Item2, out var v2) ? v2 : null!;
}

var part2 = instructions
    .Where(p => p.Key.EndsWith('A'))
    .Select(p => p.Value)
    .Select(i => GetInstructionCount(i.Name))
    .FindLowestCommonDenominator();

Console.WriteLine($"Part 1: {GetInstructionCount("AAA")}");
Console.WriteLine($"Part 2: {part2}");

int GetInstructionCount(string startInstruction)
{
    var current = instructions[startInstruction];
    var counter = instructionCounter!.GetEnumerator();
    while (current.Name[2] != 'Z')
    {
        current = counter.Current == 'L' ? current.Left : current.Right;
        counter.MoveNext();
    }

    var value = instructionCounter.Counter;
    instructionCounter.Counter = 0;

    return value;
}

file record Instruction(string Name)
{
    public Instruction Left { get; set; } = null!;
    public Instruction Right { get; set; } = null!;
}
