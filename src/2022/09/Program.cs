using AdventOfCode.Common;

var moves = Resources.GetInputFileLines();

static Coor FollowHead(Coor head, Coor tail)
{
    var diff = head - tail;
    var distanceX = Math.Abs(diff.X);
    var distanceY = Math.Abs(diff.Y);
    var distance = distanceX + distanceY;

    return (distanceY, distanceX) switch
    {
        (2, 0) => tail + ((head.Y - tail.Y) / 2, 0),
        (0, 2) => tail + (0, (head.X - tail.X) / 2),
        _ => distance <= 2
            ? tail
            : tail + (diff.Y / Math.Max(distanceY, 1), diff.X / Math.Max(distanceX, 1)),
    };
}

static int Simulate(int ropeLength, string[] moves)
{
    var directions = new Dictionary<char, Coor>
    {
        ['R'] = new Coor(0, 1),
        ['L'] = new Coor(0, -1),
        ['U'] = new Coor(1, 0),
        ['D'] = new Coor(-1, 0),
    };
    
    var positions = new HashSet<Coor> { Coor.Zero };
    var rope = new List<Coor>(Enumerable.Range(0, ropeLength).Select(_ => Coor.Zero));

    foreach (var move in moves)
    {
        var count = int.Parse(move.Substring(2));
        var dir = directions[move[0]];

        for (var i = 0; i < count; i++)
        {
            rope[0] += dir;

            for (int j = 1; j < rope.Count; j++)
            {
                rope[j] = FollowHead(rope[j - 1], rope[j]);
            }

            positions.Add(rope.Last());
        }
    }

    return positions.Count;
}

Console.WriteLine($"Part 1: {Simulate(2, moves)}");
Console.WriteLine($"Part 2: {Simulate(10, moves)}");
