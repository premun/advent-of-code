using System.Text.RegularExpressions;
using AdventOfCode.Common;

var inputRegex = new Regex(
   @"Monkey (?<name>\d):\r?\n" +
   @" +Starting items: (?<startingItems>[\d ,]+)\r?\n" +
   @" +Operation: new = (?<operation>(old|\d| |\+|\*)+)\r?\n" +
   @" +Test: divisible by (?<test>[\d]+)\r?\n" +
   @" +If true: throw to monkey (?<trueMonkey>\d+)\r?\n" +
   @" +If false: throw to monkey (?<falseMonkey>\d+)",
   RegexOptions.Multiline);

var matches = inputRegex.Matches(Resources.GetInputFileContent());
var monkeys1 = new Dictionary<int, Monkey>();
var monkeys2 = new Dictionary<int, Monkey>();

foreach (Match match in matches.Cast<Match>())
{
    var name = int.Parse(match.Groups["name"].Value);
    var items = match.Groups["startingItems"].Value.Split(", ").Select(ulong.Parse);
    var operation = match.Groups["operation"].Value;
    var test = ulong.Parse(match.Groups["test"].Value);
    var trueMonkey = int.Parse(match.Groups["trueMonkey"].Value);
    var falseMonkey = int.Parse(match.Groups["falseMonkey"].Value);

    monkeys1.Add(name, new Monkey(items, operation, test, trueMonkey, falseMonkey));
    monkeys2.Add(name, new Monkey(items, operation, test, trueMonkey, falseMonkey));
}

static ulong GetMonkeyBusiness(Dictionary<int, Monkey> monkeys, int rounds, Func<ulong, ulong> decreaseWorry)
{
    for (int i = 0; i < rounds; i++)
    {
        foreach (var monkey in monkeys.Values)
        {
            monkey.Inspect(monkeys, decreaseWorry);
        }
    }

    return monkeys.Values
        .Select(m => (ulong)m.Inspections)
        .OrderDescending()
        .Take(2)
        .Aggregate(1UL, (a, b) => a * b);
}
    
ulong commonMultiplier = monkeys2.Values.Select(m => m.Test).Aggregate((a, b) => a * b);

Console.WriteLine($"Part 1: {GetMonkeyBusiness(monkeys1, 20, item => item / 3)}");
Console.WriteLine($"Part 2: {GetMonkeyBusiness(monkeys2, 10000, item => item % commonMultiplier)}");

class Monkey
{
    private readonly ulong? _operand1;
    private readonly ulong? _operand2;
    private readonly bool _isAddition;
    private readonly int _trueMonkey;
    private readonly int _falseMonkey;

    public Queue<ulong> Items { get; }

    public ulong Test { get; }

    public int Inspections { get; private set; }

    public Monkey(IEnumerable<ulong> items, string operation, ulong test, int trueMonkey, int falseMonkey)
    {
        Items = new(items);

        var parts = operation.Split(" ");
        _operand1 = parts[0] == "old" ? null : ulong.Parse(parts[0]);
        _operand2 = parts[2] == "old" ? null : ulong.Parse(parts[2]);
        _isAddition = parts[1] == "+";

        Test = test;
        _trueMonkey = trueMonkey;
        _falseMonkey = falseMonkey;
    }

    public void Inspect(Dictionary<int, Monkey> monkeys, Func<ulong, ulong> decreaseWorry)
    {
        while (Items.TryDequeue(out var item))
        {
            Inspections++;
            item = decreaseWorry(ApplyOperation(item));
            monkeys[GetMonkey(item)].Items.Enqueue(item);
        }
    }

    private ulong ApplyOperation(ulong value)
    {
        var op1 = _operand1 ?? value;
        var op2 = _operand2 ?? value;
        return _isAddition ? op1 + op2 : op1 * op2;
    }

    private int GetMonkey(ulong value) => value % Test == 0 ? _trueMonkey : _falseMonkey;
}
