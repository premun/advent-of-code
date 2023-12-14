using AdventOfCode.Common;
using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileLines()
    .ParseAsArray();

var height = map.GetLength(0);
var width = map.GetLength(1);

Tilt(Coor.Up);

map.Print();
Console.WriteLine();
Console.WriteLine($"Part 1: {GetWeight()}");

void Tilt(Coor direction)
{
    bool movement;
    do
    {
        movement = false;
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (map![row, col] != 'O') continue;

                var c = new Coor(row, col) + direction;
                while (c.InBoundsOf(map) && map.Get(c) == '.')
                {
                    map.Set(c - direction, '.');
                    map.Set(c, 'O');
                    movement = true;
                    c += direction;
                }
            }
        }
    } while (movement);
}

int GetWeight() => map!
    .AllCoordinates()
    .Select(c => map.Get(c) == 'O' ? height - c.Row : 0)
    .Sum();
