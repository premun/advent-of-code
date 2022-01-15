using System.Collections.ObjectModel;

namespace AdventOfCode._2020_16;

class Field
{
    public string Name { get; }

    public ReadOnlyCollection<(int Min, int Max)> Intervals { get; }

    public Field(string name, ReadOnlyCollection<(int Min, int Max)> intervals)
    {
        Name = name;
        Intervals = intervals;
    }

    public bool Includes(int number)
    {
        return Intervals.Any(i => number >= i.Min && number <= i.Max);
    }

    public static Field FromString(string definition)
    {
        var parts = definition.Split(':');
        var intervals = parts[1].Trim().Split(" or ");

        return new Field(
            parts[0],
            intervals
                .Select(i =>
                {
                    var numbers = i.Split('-').Select(int.Parse);
                    return (Min: numbers.ElementAt(0), Max: numbers.ElementAt(1));
                })
                .ToList()
                .AsReadOnly());
    }
}

static class FieldExtensions
{
    public static IEnumerable<string> GetMatching(this Dictionary<string, Field> fields, int number)
    {
        return fields
            .Where(f => f.Value.Includes(number))
            .Select(f => f.Key);
    }
}
