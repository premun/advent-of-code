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
                    return current;
                }
            }
            else
            {
                return null;
            }
        }
    }

    return null;
}

int Part1(string[] lines)
{
    return lines
        .Select(FindCorruptedChar)
        .Where(c => c.HasValue)
        .Select(c => c switch
        {
            ')' => 3,
            ']' => 57,
            '}' => 1197,
            '>' => 25137,
            _ => throw new InvalidOperationException("???"),
        })
        .Sum();
}

Console.WriteLine($"Part 1: {Part1(lines)}");
