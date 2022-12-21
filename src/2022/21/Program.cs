using System.Text.RegularExpressions;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();
var monkeys = new Dictionary<string, Monkey>();
var operationMonkeyRegex = new Regex(@"^(?<name>[a-z]{4}): (?<monkey1>(\d+|[a-z]{4})) (?<operation>\-|\+|\/|\*) (?<monkey2>(\d+|[a-z]{4}))$");

foreach (var line in lines)
{
    var name = line.Substring(0, 4);
    var match = operationMonkeyRegex.Match(line);
    if (match.Success)
    {
        var m1 = match.Groups["monkey1"].Value;
        var m2 = match.Groups["monkey2"].Value;

        var monkey1 = int.TryParse(m1, out var number1)
            ? new NumberMonkey(name, number1)
            : (Monkey)new MonkeyRef(name, m1, monkeys);

        var monkey2 = int.TryParse(m2, out var number2)
            ? new NumberMonkey(name, number2)
            : (Monkey)new MonkeyRef(name, m2, monkeys);

        monkeys.Add(name, new OperationMonkey(name, monkey1, monkey2, match.Groups["operation"].Value[0]));
    }
    else
    {
        monkeys.Add(name, new NumberMonkey(name, int.Parse(line.Substring(6))));
    }
}

static Monkey? FindMonkey(Monkey monkey, string name)
{
    if (monkey.Name == name)
    {
        return monkey;
    }

    return monkey switch
    {
        MonkeyRef monkeyRef => FindMonkey(monkeyRef.Monkey, name),
        OperationMonkey operationMonkey =>
            FindMonkey(operationMonkey.Monkey1, name) ??
            FindMonkey(operationMonkey.Monkey2, name),
        _ => null,
    };
};

static long GetValueForHumnResult(Monkey monkey, long goal, string targetMonkey)
{
    if (monkey.Name == targetMonkey)
    {
        return goal;
    }

    if (monkey is MonkeyRef monkeyRef)
        return GetValueForHumnResult(monkeyRef.Monkey, goal, targetMonkey);

    if (monkey is not OperationMonkey operationMonkey)
        throw new InvalidOperationException($"Monkey {monkey.Name} is not an operation monkey");
    
    var humnLeft = FindMonkey(operationMonkey.Monkey1, targetMonkey) != null;
    Monkey monkeyDependingOnTarget;
    long newGoal;
    if (humnLeft)
    {
        monkeyDependingOnTarget = operationMonkey.Monkey1;
        var valueRight = operationMonkey.Monkey2.GetValue();
        newGoal = operationMonkey.Operation switch
        {
            '+' => goal - valueRight,
            '-' => goal + valueRight,
            '*' => goal / valueRight,
            '/' => goal * valueRight,
            _ => throw new InvalidOperationException()
        };
    }
    else
    {
        monkeyDependingOnTarget = operationMonkey.Monkey2;
        var valueLeft = operationMonkey.Monkey1.GetValue();
        newGoal = operationMonkey.Operation switch
        {
            '+' => goal - valueLeft,
            '-' => valueLeft - goal,
            '*' => goal / valueLeft,
            '/' => valueLeft / goal,
            _ => throw new InvalidOperationException()
        };
    }

    return GetValueForHumnResult(monkeyDependingOnTarget, newGoal, targetMonkey);
};

var rootMonkey = (OperationMonkey)monkeys["root"];

Console.WriteLine("Part 1: " + rootMonkey.GetValue());

const string Human = "humn";

var humnLeft = FindMonkey(rootMonkey.Monkey1, Human) != null;
if (humnLeft)
{
    Console.WriteLine("Part 2: " + GetValueForHumnResult(rootMonkey.Monkey1, rootMonkey.Monkey2.GetValue(), Human));
}
else
{
    Console.WriteLine("Part 2: " + GetValueForHumnResult(rootMonkey.Monkey2, rootMonkey.Monkey1.GetValue(), Human));
}

abstract record Monkey(string Name)
{
    public abstract long GetValue();
}

record MonkeyRef(string Name, string OtherMonkey, Dictionary<string, Monkey> Monkeys)
    : Monkey(Name)
{
    public Monkey Monkey => Monkeys[OtherMonkey];

    public override long GetValue() => Monkey.GetValue();
}

record NumberMonkey(string Name, int Number)
    : Monkey(Name)
{
    public override long GetValue() => Number;
}

record OperationMonkey(string Name, Monkey Monkey1, Monkey Monkey2, char Operation)
    : Monkey(Name)
{
    public override long GetValue() => Operation switch
    {
        '+' => Monkey1.GetValue() + Monkey2.GetValue(),
        '-' => Monkey1.GetValue() - Monkey2.GetValue(),
        '*' => Monkey1.GetValue() * Monkey2.GetValue(),
        '/' => Monkey1.GetValue() / Monkey2.GetValue(),
        _ => throw new Exception("Unknown operation"),
    };
}
