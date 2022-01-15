using AdventOfCode._2020_16;
using AdventOfCode.Common;

var lines = Resources.GetResourceFileLines("input.txt");

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
    .ToArray();

var invalidRate = nearbyTickets
    .SelectMany(n => n)
    .Where(n => !fields.GetMatching(n).Any())
    .Sum();

Console.WriteLine($"Part 1: {invalidRate}");

var fieldMapping = new Dictionary<int, string>();
var allTickets = nearbyTickets
    .Where(t => t.All(n => fields.GetMatching(n).Any()))
    .Append(myTicket)
    .ToArray();

while (fieldMapping.Count < myTicket.Length)
{
    var unmappedFields = Enumerable.Range(0, myTicket.Length)
        .Except(fieldMapping.Keys.ToArray())
        .ToArray();

    foreach (var ticket in allTickets)
    {
        bool found = false;

        for (int i = 0; i < unmappedFields.Length; i++)
        {
            var number = ticket[unmappedFields[i]];
            var belongsTo = fields.GetMatching(number).ToList();

            if (belongsTo.Count == 1)
            {
                Console.WriteLine($"Mapping {belongsTo[0]} to {i}");
                fieldMapping.Add(unmappedFields[i], belongsTo[0]);
                found = true;
                break;
            }
        }

        if (found == true)
        {
            break;
        }
    }
}

var departures = fieldMapping
        .Where(p => p.Value.StartsWith("departure "))
        .Select(p => myTicket[p.Key])
        .Aggregate((n, acc) => n * acc);

Console.WriteLine($"Part 2: {departures}");
