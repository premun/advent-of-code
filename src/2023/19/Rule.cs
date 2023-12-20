using System.Diagnostics.CodeAnalysis;

abstract record Rule(Func<Part, int> Selector, bool LessThan, int Target, string Workflow)
{
    public bool TryAccept(Part part, [NotNullWhen(true)] out string? nextWorkflow)
    {
        var value = Selector(part);
        var matches = LessThan
            ? value < Target
            : value > Target;

        if (!matches)
        {
            nextWorkflow = null;
            return false;
        }

        nextWorkflow = Workflow;
        return true;
    }

    public static Rule FromDefinition(string definition)
    {
        bool lessThan;
        if (definition.Contains('<'))
        {
            lessThan = true;
        }
        else if (definition.Contains('>'))
        {
            lessThan = false;
        }
        else
        {
            return new OtherwiseRule(definition);
        }

        var parts = definition.Split('<', '>', ':');

        return definition[0] switch
        {
            'x' => new XRule(int.Parse(parts[1]), lessThan, parts[2]),
            'm' => new MRule(int.Parse(parts[1]), lessThan, parts[2]),
            'a' => new ARule(int.Parse(parts[1]), lessThan, parts[2]),
            's' => new SRule(int.Parse(parts[1]), lessThan, parts[2]),
            _ => throw new Exception()
        };
    }
}

record XRule(int target, bool lessThan, string workflow)
    : Rule(p => p.X, lessThan, target, workflow);

record MRule(int target, bool lessThan, string workflow)
    : Rule(p => p.M, lessThan, target, workflow);

record ARule(int target, bool lessThan, string workflow)
    : Rule(p => p.A, lessThan, target, workflow);

record SRule(int target, bool lessThan, string workflow)
    : Rule(p => p.S, lessThan, target, workflow);

record OtherwiseRule(string workflow)
    : Rule(_ => 0, true, 1, workflow);
