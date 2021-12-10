using Common;

var lines = Resources.GetResourceFileLines("input.txt");

Dictionary<char, char> pairs = new()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' },
};

char? FindCorruptedChar(string line)
{
    var stack = new Stack<char>();

    foreach (var current in line)
    {
        if (pairs.ContainsKey(current))
        {
            // current is opening bracket
            stack.Push(current);
        }
        else
        {
            // current is closing bracket
            if (stack.TryPop(out var opening))
            {
                if (!pairs.ContainsKey(opening) || current != pairs[opening])
                {
                    // Wrong bracket => corrupted line
                    return current;
                }
            }
            else
            {
                // Nothing to pop => corrupted line
                return current;
            }
        }
    }

    return null;
}

IEnumerable<char> CompleteLine(string line)
{
    var stack = new Stack<char>();

    foreach (var current in line)
    {
        if (pairs.ContainsKey(current))
        {
            // current is opening bracket
            stack.Push(current);
        }
        else
        {
            // current is closing bracket
            if (stack.TryPop(out var opening))
            {
                if (!pairs.ContainsKey(opening) || current != pairs[opening])
                {
                    // Wrong bracket => corrupted line
                    yield break;
                }
            }
            else
            {
                // Nothing to pop => corrupted line
                yield break;
            }
        }
    }

    while (stack.TryPop(out var c))
    {
        yield return pairs[c];
    }
}

static int ScoreCorruption(char? c)
{
    if (!c.HasValue)
    {
        return 0;
    }

    return c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new InvalidOperationException("???"),
    };
}

long ScoreCompletion(IEnumerable<char> missing)
{
    if (!missing.Any())
    {
        return 0;
    }

    return missing
        .Select(c => c switch
        {
            ')' => 1L,
            ']' => 2L,
            '}' => 3L,
            '>' => 4L,
            _ => throw new InvalidOperationException("???"),
        })
        .Aggregate((acc, score) => acc * 5 + score);
}

Console.WriteLine($"Part 1: {lines.Select(FindCorruptedChar).Select(ScoreCorruption).Sum()}");

var scores = lines
    .Select(CompleteLine)
    .Select(ScoreCompletion)
    .Where(s => s > 0)
    .OrderBy(s => s)
    .ToArray();

Console.WriteLine($"Part 2: {scores[scores.Length / 2]}");
