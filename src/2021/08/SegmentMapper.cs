namespace AdventOfCode._2021_08;

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

    private static readonly char[] s_segments = ['a', 'b', 'c', 'd', 'e', 'f', 'g'];

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

    private readonly Dictionary<char, char> _segmentMapping = [];

    public void ResolveDigits(string[] signals)
    {
        var orderedSignals = signals
            .OrderBy(digit => digit.Length is 2 or 3 or 4 or 7 ? -1 : 1) // Optimization
            .ToDictionary(d => d, d => FindMappings(d).ToHashSet());

        var disallowedMappings = FindDisallowedMappings(orderedSignals, [])
            ?? throw new Exception($"Unclear mappings for {string.Join(", ", signals)}");

        foreach (var c in s_segments)
        {
            _segmentMapping[c] = s_segments.First(d => !disallowedMappings.Contains(new Mapping(c, d)));
        }
    }

    private static HashSet<Mapping>? FindDisallowedMappings(
        IEnumerable<KeyValuePair<string, HashSet<string>>> signals,
        HashSet<Mapping> mappings)
    {
        if (!signals.Any())
        {
            // We found a 1:1 mapping for every segment
            if (mappings.Count == (s_segments.Length - 1) * s_segments.Length)
            if (s_segments.All(segment => mappings.Count(mapping => mapping.From == segment) == s_segments.Length - 1))
            {
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

            var result = FindDisallowedMappings(signals.Skip(1), newMappings);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    public int MapDigits(string[] digits)
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

    private static string[] FindMappings(string signal) => signal.Length switch
    {
        // This is 1
        2 => [s_digits[1]],

        // This is 7
        3 => [s_digits[7]],

        // This is 4
        4 => [s_digits[4]],

        // Can be 2, 3, 5
        5 => [s_digits[2], s_digits[3], s_digits[5]],

        // Can be 0, 6, 9
        6 => [s_digits[0], s_digits[6], s_digits[9]],

        // This is 8
        7 => [s_digits[8]],

        _ => throw new ArgumentException($"Invalid signal: '{signal}'"),
    };

    private static char[] GetDistinctChars(params string[] strings)
        => strings.SelectMany(x => x).Distinct().ToArray();
}
