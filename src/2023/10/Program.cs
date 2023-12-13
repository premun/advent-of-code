using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

char[,] pipes = Resources.GetInputFileLines().ParseAsArray();

(int Index, Coor Direction)[] directions =
[
    (2, Coor.Up),
    (3, Coor.Right),
    (0, Coor.Down),
    (1, Coor.Left),
];

var connections = new Dictionary<char, bool[]>
{
    //         Up   Right   Down   Left
    ['|'] = [ true, false,  true, false],
    ['-'] = [false,  true, false,  true],
    ['L'] = [ true,  true, false, false],
    ['J'] = [ true, false, false,  true],
    ['7'] = [false, false,  true,  true],
    ['F'] = [false,  true,  true, false],
    ['.'] = [false, false, false, false],
    ['S'] = [ true,  true,  true,  true],
};

var width = pipes.GetLength(1);
var height = pipes.GetLength(0);

var start =
    (from row in Enumerable.Range(0, height)
     from col in Enumerable.Range(0, width)
     where pipes[row, col] == 'S'
     select new Coor(row, col)).First();

var queue = new Queue<(int, Coor)>([(-1, start)]);
var distances = new int[height, width].InitializeWith(int.MaxValue);

var maxDistance = 0;

while (queue.Count != 0)
{
    var (distance, coor) = queue.Dequeue();
    var newDistance = distance + 1;
    distances.Set(coor, newDistance);
    foreach (var (i, dir) in directions)
    {
        var newCoor = coor + dir;
        if (newCoor.InBoundsOf(pipes)
            && distances.Get(newCoor) > newDistance
            && connections[pipes.Get(newCoor)][i])
        {
            queue.Enqueue((newDistance, newCoor));
            maxDistance = Math.Max(maxDistance, newDistance);
        }
    }
}

pipes.Print();
Console.WriteLine();
pipes.Print(c => distances.Get(c) < int.MaxValue ? (char)(distances.Get(c) + '0') : null);
Console.WriteLine();

Console.WriteLine($"Part 1: {maxDistance}");

