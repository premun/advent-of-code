using AdventOfCode.Common;

var equations = Resources.GetInputFileLines()
    .Select(line => line.SplitBy(": "))
    .Select(parts => new Equation(ulong.Parse(parts[0]), [.. parts[1].Split(' ').Select(ulong.Parse)]))
    .ToArray();

var validEquations = equations
    .Where(equation => equation.IsValid(useConcatenation: false))
    .Select(equation => equation.Target)
    .ToList();

var total = validEquations
    .Sum();

var totalWithConcatenation = equations
    .Where(e => !validEquations.Contains(e.Target))
    .Where(e => e.IsValid(useConcatenation: true))
    .Select(equation => equation.Target)
    .Sum();

Console.WriteLine($"Part 1: {total}");
Console.WriteLine($"Part 2: {total + totalWithConcatenation}");

file record Equation(ulong Target, ulong[] Operands)
{
    public bool IsValid(bool useConcatenation)
        => IsValid(useConcatenation, 0, 0);

    private bool IsValid(bool useConcatenation, ulong acc, int index)
    {
        if (index == Operands.Length) return acc == Target;
        if (acc > Target) return false;

        return IsValid(useConcatenation, Math.Max(acc, 1) * Operands[index], index + 1)
            || IsValid(useConcatenation, acc + Operands[index], index + 1)
            || (useConcatenation && IsValid(true, Concatenate(acc, Operands[index]), index + 1));
    }

    // 1000 with as many zeros as a base-10 log (and account for '0')
    private static ulong Concatenate(ulong first, ulong second)
        => first * (ulong)Math.Max(10, Math.Pow(10, Math.Ceiling(Math.Log10(second)))) + second;
}
