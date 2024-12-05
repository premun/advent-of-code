using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var rules = lines
    .TakeWhile(l => l.Contains('|'))
    .Select(line => line.SplitToNumbers("|"))
    .Select(numbers => (First: numbers[0], Second: numbers[1]));

var updates = lines
    .SkipWhile(l => l.Contains('|'))
    .Select(line => line.SplitToNumbers());

var dependentOn = new Dictionary<int, HashSet<int>>();
foreach (var (former, latter) in rules)
{
    if (!dependentOn.TryGetValue(latter, out HashSet<int>? deps))
    {
        deps = [];
        dependentOn[latter] = deps;
    }

    deps.Add(former);
}

Console.WriteLine($"Part 1: {GetMiddlePage(false, dependentOn, updates)}");
Console.WriteLine($"Part 2: {GetMiddlePage(true, dependentOn, updates)}");

static int GetMiddlePage(bool takeIncorrectlyReordered, Dictionary<int, HashSet<int>> rules, IEnumerable<int[]> updates)
{
    var result = 0;
    foreach (var update in updates)
    {
        var isCorrect = true;
        for (int i = 0; i < update.Length; i++)
        {
        Again:
            var shouldBeEarlier = rules[update[i]];
            for (int j = i + 1; j < update.Length; ++j)
            {
                if (shouldBeEarlier.Contains(update[j]))
                {
                    isCorrect = false;
                    if (takeIncorrectlyReordered)
                    {
                        (update[i], update[j]) = (update[j], update[i]);
                        i = 0;
                        goto Again;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (!takeIncorrectlyReordered && !isCorrect)
            {
                break;
            }
        }

        if (takeIncorrectlyReordered != isCorrect)
        {
            result += update[update.Length / 2];
        }
    }

    return result;
}
