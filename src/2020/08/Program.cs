using AdventOfCode._2020_08;
using AdventOfCode.Common;

(Operation Operation, int Argument)[] instructions = Resources.GetResourceFileLines("input.txt")
    .Select(line =>
    {
        var parts = line.Split(' ');
        var value = int.Parse(parts[1].Replace("+", string.Empty));
        var kind = parts[0] switch
        {
            "nop" => Operation.Nop,
            "acc" => Operation.Acc,
            "jmp" => Operation.Jmp,
            _ => throw new NotImplementedException()
        };

        return (kind, value);
    })
    .ToArray();

static int RunProgram((Operation Operation, int Argument)[] instructions)
{
    int pc = 0;
    int acc = 0;
    var runInstructions = new bool[instructions.Length];

    while (pc < instructions.Length)
    {
        if (runInstructions[pc])
        {
            throw new ProgramLoopException(acc);
        }

        runInstructions[pc] = true;

        switch (instructions[pc].Operation)
        {
            case Operation.Nop:
                pc++;
                break;

            case Operation.Acc:
                acc += instructions[pc].Argument;
                pc++;
                break;

            case Operation.Jmp:
                pc += instructions[pc].Argument;
                break;
        }
    }

    return acc;
}

try
{
    int acc = RunProgram(instructions);
}
catch (ProgramLoopException e)
{
    Console.WriteLine($"Part 1: {e.AccumulatorValue}");
}

for (int i = 0; i < instructions.Length; i++)
{
    var tmp = instructions[i];

    switch (instructions[i].Operation)
    {
        case Operation.Nop:
            instructions[i] = (Operation.Jmp, tmp.Argument);
            break;

        case Operation.Acc:
            continue;

        case Operation.Jmp:
            instructions[i] = (Operation.Nop, tmp.Argument);
            break;
    }

    try
    {
        Console.WriteLine($"Part 2: {RunProgram(instructions)}");
    }
    catch (ProgramLoopException)
    {
    }

    instructions[i] = tmp;
}
