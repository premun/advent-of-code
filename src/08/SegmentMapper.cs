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
    private record Mapping(char From, char To);

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

    private readonly Dictionary<char, char> _segmentMapping = new();

    public void ResolveDigits(string[] signals)
    {
        var orderedSignals = signals
            .OrderBy(digit => digit.Length == 2 || digit.Length == 3 || digit.Length == 4 || digit.Length == 7 ? -1 : 1)
            .ToDictionary(d => d, d => FindMappings(d).ToHashSet());

        var disallowedMappings = FindMapping(orderedSignals, new HashSet<Mapping>());

        if (disallowedMappings == null)
        {
            throw new Exception($"Unclear mappings for {string.Join(", ", signals)}");
        }

        foreach (var c in s_segments)
        {
            _segmentMapping[c] = s_segments.First(d => !disallowedMappings.Contains(new Mapping(c, d)));
        }
    }

    private HashSet<Mapping>? FindMapping(
        IEnumerable<KeyValuePair<string, HashSet<string>>> signals,
        HashSet<Mapping> mappings)
    {
        if (!signals.Any())
        {
            if (mappings.Count == (s_segments.Length - 1) * s_segments.Length)
            {
                // We found a 1:1 mapping for every segment
                return mappings;
            }

            return null;
        }

        var pair = signals.First();
        var signal = pair.Key;
        var possibleDigits = pair.Value;

        foreach (var possibleDigit in possibleDigits)
        {
            var mappedSegments = GetDistinctChars(possibleDigit);
            var unmappedSegments = s_segments.Except(mappedSegments);
            var newMappings = new HashSet<Mapping>(mappings);

            foreach (var segment in signal)
            {
                foreach (var unmappedSegment in unmappedSegments)
                {
                    newMappings.Add(new Mapping(segment, unmappedSegment));
                }
            }

            foreach (var unmappedSignal in s_segments.Except(signal))
            {
                foreach (var mappedSegment in mappedSegments)
                {
                    newMappings.Add(new Mapping(unmappedSignal, mappedSegment));
                }
            }

            var result = FindMapping(signals.Skip(1), newMappings);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    public int MapDigit(string[] digits)
    {
        var result = 0;

        foreach (var digit in digits)
        {
            // Map to our digit
            var mappedDigit = new string(digit.Select(x => _segmentMapping[x]).OrderBy(x => x).ToArray());

            var value = s_digits.First(x => x.Value == mappedDigit).Key;

            result *= 10;
            result += value;
        }

        return result;
    }

    private static IEnumerable<string> FindMappings(string signal)
    {
        switch (signal.Length)
        {
            // This is 1
            case 2:
                return new[] { s_digits[1] };

            // This is 7
            case 3:
                return new[] { s_digits[7] };

            // This is 4
            case 4:
                return new[] { s_digits[4] };

            // Can be 2, 3, 5
            case 5:
                return new[] { s_digits[2], s_digits[3], s_digits[5] };

            // Can be 0, 6, 9
            case 6:
                return new[] { s_digits[0], s_digits[6], s_digits[9] };

            // This is 8
            case 7:
                return new[] { s_digits[8] };
        }

        throw new ArgumentException($"Invalid signal: '{signal}'");
    }

    private static IEnumerable<char> GetDistinctChars(params string[] strings)
        => strings.SelectMany(x => x).Distinct().ToArray();
}
