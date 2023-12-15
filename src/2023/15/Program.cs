using AdventOfCode.Common;

var instructions = Resources.GetInputFileLines()
    .Single()
    .SplitBy(",");

var boxes = Enumerable.Range(0, 256)
    .Select(_ => new LinkedList<Lens>())
    .ToArray();

foreach (var instruction in instructions)
{
    var index1 = instruction.IndexOf('=');
    var index2 = instruction.IndexOf('-');
    var label = instruction[0..Math.Max(index1, index2)];
    var box = boxes[Hash(label)];
    var lens = box.FirstOrDefault(lens => lens.Label == label);

    if (index1 != -1) // =
    {
        var length = int.Parse(instruction.Substring(index1 + 1));
        if (lens != null)
        {
            lens.FocalLength = length;
        }
        else
        {
            box.AddLast(new Lens(label, length));
        }
    }
    else if (lens != null) // -
    {
        box.Remove(lens);
    }
}

var part1 = instructions
    .Select(Hash)
    .Sum();

var part2 = boxes
    .SelectMany((box, boxId) =>
        box.Select((lens, lensId) => (1 + boxId) * (lensId + 1) * lens.FocalLength))
    .Sum();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static int Hash(string input)
    => input.Aggregate(0, (acc, c) => acc = (acc + c) * 17 % 256);

file class Lens(string label, int focalLength)
{
    public string Label { get; } = label;
    public int FocalLength { get; set; } = focalLength;
}
