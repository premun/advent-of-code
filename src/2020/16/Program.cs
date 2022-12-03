using AdventOfCode._2020_16;
using AdventOfCode.Common;

var lines = Resources.GetInputFileLines();

var fields = lines
    .TakeWhile(l => l != "your ticket:")
    .Select(Field.FromString)
    .ToDictionary(f => f.Name);

var myTicket = lines
    .SkipWhile(l => l != "your ticket:")
    .ElementAt(1)
    .SplitToNumbers();

var nearbyTickets = lines
    .SkipWhile(l => l != "nearby tickets:")
    .Skip(1)
    .Select(l => l.SplitToNumbers())
    .ToList();

var invalidRate = nearbyTickets
    .SelectMany(n => n)
    .Where(n => !fields.GetMatching(n).Any())
    .Sum();

Console.WriteLine($"Part 1: {invalidRate}");

var fieldMapping = new Dictionary<int, string>();
var validTickets = nearbyTickets
    .Where(t => t.All(n => fields.GetMatching(n).Any()))
    .Append(myTicket)
    .ToList();

while (fieldMapping.Count < myTicket.Length)
{
    var unmappedFields = Enumerable.Range(0, myTicket.Length)
        .Except(fieldMapping.Keys.ToList())
        .ToList();

    foreach (var unmappedField in unmappedFields)
    {
        var matchingFields = validTickets
            .Select(t => t[unmappedField])
            .Select(n => fields.GetMatching(n).Select(f => f.Name).Except(fieldMapping.Values).ToList());

        var intersection = matchingFields.First();
        foreach (var matching in matchingFields.Skip(1))
        {
            intersection = intersection.Intersect(matching).ToList();

            if (intersection.Count == 1)
            {
                break;
            }
        }

        if (intersection.Count == 1)
        {
            // Console.WriteLine($"Mapping {intersection[0]} to {unmappedField}");
            fieldMapping.Add(unmappedField, intersection[0]);
            break;
        }
    }
}

var departures = fieldMapping
    .Where(p => p.Value.StartsWith("departure "))
    .Select(p => (long)myTicket[p.Key])
    .Aggregate((n, acc) => n * acc);

Console.WriteLine($"Part 2: {departures}");
