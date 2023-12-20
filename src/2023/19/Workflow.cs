record Workflow(string Name, IReadOnlyCollection<Rule> Rules)
{
    public string Evaluate(Part part)
    {
        foreach (var rule in Rules)
        {
            if (rule.TryAccept(part, out var next))
            {
                return next;
            }
        }

        throw new Exception($"Workflow {Name} failed to evaluate part {part}");
    }

    public static Workflow FromDefinition(string definition)
    {
        var brace = definition.IndexOf('{');
        var name = definition.Substring(0, brace);
        var rules = definition[(brace + 1)..^1]
            .Split(',')
            .Select(Rule.FromDefinition)
            .ToList();

        return new Workflow(name, rules);
    }
}
