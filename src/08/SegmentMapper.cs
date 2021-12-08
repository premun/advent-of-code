namespace _08;

/*
      0:      1:      2:      3:      4:
     aaaa    ....    aaaa    aaaa    ....
    b    c  .    c  .    c  .    c  b    c
    b    c  .    c  .    c  .    c  b    c
     ....    ....    dddd    dddd    dddd
    e    f  .    f  e    .  .    f  .    f
    e    f  .    f  e    .  .    f  .    f
     gggg    ....    gggg    gggg    ....

      5:      6:      7:      8:      9:
     aaaa    aaaa    aaaa    aaaa    aaaa
    b    .  b    .  .    c  b    c  b    c
    b    .  b    .  .    c  b    c  b    c
     dddd    dddd    ....    dddd    dddd
    .    f  e    f  .    f  e    f  .    f
    .    f  e    f  .    f  e    f  .    f
     gggg    gggg    ....    gggg    gggg
*/

class SegmentMapper
{
    private static readonly char[] s_segments = { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

    private static readonly Dictionary<int, string> s_digits = new()
    {
        { 0, "abcefg" },
        { 1, "cf" },
        { 2, "acdeg" },
        { 3, "acdfg" },
        { 4, "bcdf" },
        { 5, "abdfg" },
        { 6, "abdefg" },
        { 7, "acf" },
        { 8, "abcdefg" },
        { 9, "abcdfg" },
    };

    private readonly Dictionary<char, HashSet<char>> _possibleMappings;

    public SegmentMapper()
    {
        _possibleMappings = new();

        foreach (var segment in s_segments)
        {
            _possibleMappings.Add(segment, new HashSet<char>(s_segments));
        }
    }

    public void TeachDigit(string digit)
    {
        switch (digit.Length)
        {
            // This is 1
            case 2:
                RestrictMappings(digit, s_digits[1]);
                return;

            // This is 7
            case 3:
                RestrictMappings(digit, s_digits[7]);
                return;

            // This is 4
            case 4:
                RestrictMappings(digit, s_digits[4]);
                return;

            // Can be 2, 3, 5
            case 5:
                RestrictMapping(digit, new[] { s_digits[2], s_digits[3], s_digits[5] });
                return;

            // Can be 0, 6, 9
            case 6:
                RestrictMapping(digit, new[] { s_digits[0], s_digits[6], s_digits[9] });
                return;

            // This is 8 - no info
            case 7:
                return;
        }

        throw new ArgumentException($"Invalid digit '{digit}'");
    }

    public int MapDigit(string[] digits)
    {
        if (_possibleMappings.Values.Any(m => m.Count != 1))
        {
            throw new InvalidOperationException("Mapping is unclear!");
        }

        return 0;
    }

    private void AddMapping(char from, char to) => _possibleMappings[from].Add(to);

    private void RemoveMapping(char from, char to) => _possibleMappings[from].Remove(to);

    private void RestrictMappings(string from, string to)
    {
        RemoveAllMappings(from);

        foreach (var f in from)
        {
            foreach (var t in to)
            {
                AddMapping(f, t);
            }
        }
    }

    private void RestrictMapping(string from, string[] to)
    {
        var chars = to.SelectMany(x => x).Distinct().ToArray();

        foreach (var c in from)
        {
            _possibleMappings[c] = _possibleMappings[c].Intersect(chars).ToHashSet();
        }
    }

    private void RemoveAllMappings(string digit)
    {
        foreach (var c in digit)
        {
            RemoveAllMappings(c);
        }
    }

    private void RemoveAllMappings(char to)
    {
        foreach (var segment in s_segments)
        {
            RemoveMapping(segment, to);
        }
    }
}
