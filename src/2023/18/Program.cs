using System.Text.RegularExpressions;
using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var instructions = Resources.GetInputFileLines()
    .Select(Instruction.Parse)
    .ToList();

Console.WriteLine($"Part 1: {LavaLagoonMeasurer.MeasureLagoon(instructions)}");
Console.WriteLine($"Part 2: {LavaLagoonMeasurer.MeasureLagoon(instructions.Select(i => i.Invert()))}");

record Instruction(Coor Direction, int Distance, string Color)
{
    private static readonly Regex s_regex = new(@"(?<direction>R|D|L|U) (?<distance>[0-9]+) \(#(?<color>[0-9a-h]{6})\)");

    public Instruction Invert() => new(
        Color.Last() switch
        {
            '0' => Coor.Right,
            '1' => Coor.Down,
            '3' => Coor.Up,
            '2' => Coor.Left,
            _ => throw new Exception()
        },
        Convert.ToInt32(new string(Color.Take(Color.Length - 1).ToArray()), 16),
        Color);

    public static Instruction Parse(string s)
    {
        var match = s_regex.Match(s);
        return new Instruction(
            match.Groups["direction"].Value[0] switch
            {
                'R' => Coor.Right,
                'D' => Coor.Down,
                'U' => Coor.Up,
                'L' => Coor.Left,
                _ => throw new Exception()
            },
            int.Parse(match.Groups["distance"].Value),
            match.Groups["color"].Value);
    }
}
