using _10;
using Common;

var lines = Resources.GetResourceFileLines("input.txt");

Dictionary<char, char> pairs = new()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' },
};

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
                    throw new CorruptedLineException(current);
                }
            }
            else
            {
                // Nothing to pop => corrupted line
                throw new CorruptedLineException(current);
            }
        }
    }

    return stack.Select(c => pairs[c]);
}

static int ScoreCorruption(char c)
{
    return c switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new InvalidOperationException("???"),
    };
}

static long ScoreCompletion(IEnumerable<char> missing)
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

long corruptionScore = 0;
List<long> completionScores = new();

foreach (var line in lines)
{
    try
    {
        completionScores.Add(ScoreCompletion(CompleteLine(line)));
    }
    catch (CorruptedLineException e)
    {
        corruptionScore += ScoreCorruption(e.CorruptedChar);
    }
}

var scores = completionScores
    .Where(s => s > 0)
    .OrderBy(s => s)
    .ToArray();

Console.WriteLine($"Part 1: {corruptionScore}");
Console.WriteLine($"Part 2: {scores[scores.Length / 2]}");
