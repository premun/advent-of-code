using AdventOfCode.Common;

// Gather left and right lists
(int[] Left, int[] Right) lists = Resources.GetInputFileLines("input.txt")
    .Select(line => line.SplitToNumbers(" "))
    .Aggregate((Array.Empty<int>(), Array.Empty<int>()), (lists, pair) => ([.. lists.Item1, pair.First()], [.. lists.Item2, pair.Last()]));

var distances = from pair in lists.Left.Order().Zip(lists.Right.Order())
                select Math.Abs(pair.First - pair.Second);

var similarity = from left in lists.Left
                 select lists.Right.Where(right => right == left).Count() * left;

Console.WriteLine($"Part 1: {distances.Sum()}");
Console.WriteLine($"Part 2: {similarity.Sum()}");
