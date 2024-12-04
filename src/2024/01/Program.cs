using AdventOfCode.Common;

int[] Left = GetColumn(0);
int[] Right = GetColumn(1);

var distances = from pair in Left.Order().Zip(Right.Order())
                select Math.Abs(pair.First - pair.Second);

var similarity = from left in Left
                 select Right.Count(right => right == left) * left;

Console.WriteLine($"Part 1: {distances.Sum()}");
Console.WriteLine($"Part 2: {similarity.Sum()}");

static int[] GetColumn(int index) => Resources.GetInputFileLines()
    .Select(line => line.SplitToNumbers(" "))
    .Select(pair => pair[index])
    .ToArray();
