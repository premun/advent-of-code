using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

char[,] map = Resources.GetInputFileLines().ParseAsArray();
const char O = 'O';

var height = map.Height();
var width = map.Width();

Tilt(Coor.Up);

Console.WriteLine($"Part 1: {GetWeight()}");

map = Resources.GetInputFileLines().ParseAsArray();

var iterations = 1_000_000_000;
var knownStates = new Dictionary<string, int>();
var memoize = true;

for (int iteration = 1; iteration <= iterations; iteration++)
{
    SpinCycle();

    if (!memoize) continue;

    var hash = map.ToFlatString();
    if (knownStates.TryGetValue(hash, out var i))
    {
        // A cycle of the length of (iteration - i) will repeat now, we will just run the tail
        iteration = iterations - ((iterations - iteration) % (iteration - i));
        memoize = false;
    }
    else
    {
        knownStates.Add(hash, iteration);
    }
}

Console.WriteLine($"Part 2: {GetWeight()}");

void SpinCycle()
{
    Tilt(Coor.Up);
    Tilt(Coor.Left);
    Tilt(Coor.Down);
    Tilt(Coor.Right);
}

void Tilt(Coor direction)
{
    bool movement;
    do
    {
        movement = false;
        map.ForEach((row, col, value) =>
        {
            if (value != O) return;

            var c = new Coor(row, col) + direction;
            while (c.InBoundsOf(map) && map.Get(c) == '.')
            {
                map.Set(c - direction, '.');
                map.Set(c, O);
                movement = true;
                c += direction;
            }
        });
    } while (movement);
}

int GetWeight() => map
    .AllCoordinates()
    .Select(c => map.Get(c) == O ? height - c.Row : 0)
    .Sum();
