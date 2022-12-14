using AdventOfCode.Common;

var rockDefinitions = Resources.GetInputFileLines()
    .Select(l => l.Split(" -> "))
    .Select(l => l.Select(c => c.Split(',')))
    .Select(l => l.Select(c => new Coor(int.Parse(c[1]), int.Parse(c[0]) - 1)).ToArray())
    .ToArray();

var allRocks = rockDefinitions.SelectMany(r => r);

var min = new Coor(0, allRocks.Min(c => c.X));
var max = new Coor(allRocks.Max(c => c.Y), allRocks.Max(c => c.X));
var map = new bool?[max.Y - min.Y + 1, max.X - min.X + 1]; // null/false/true for empty/wall/sand
var height = map.GetLength(0);
var width = map.GetLength(1);

// Draw the map
foreach (var definitions in rockDefinitions)
{
    var prev = definitions[0] - min;
    map.Set(prev, false);

    foreach (var c in definitions.Skip(1))
    {
        var coor = c - min;
        var change = prev.X == coor.X
            ? new Coor(coor.Y < prev.Y ? -1 : 1, 0)
            : new Coor(0, coor.X < prev.X ? -1 : 1);

        do
        {
            prev += change;
            map.Set(prev, false);
        } while (prev != coor);
    }
}

// Compute by how much we need to extend the map
var extensionLeft =
   (from row in Enumerable.Range(0, height)
    from col in Enumerable.Range(0, width)
    where map[row, col].HasValue
    select height - row + col).Max() + 1;

var extensionRight =
   (from row in Enumerable.Range(0, height)
    from col in Enumerable.Range(0, width)
    where map[row, col].HasValue
    select height - row + width - col).Max() + 1;

// Copy the drawn map to the extended one
var extendedMap = new bool?[height + 1, width + extensionLeft + extensionRight];
for (var y = 0; y < map.GetLength(0); y++)
for (var x = 0; x < map.GetLength(1); x++)
{
    if (map[y, x].HasValue)
    {
        extendedMap[y, x + extensionLeft] = map[y, x];
    }
}

Console.WriteLine(SimulateSand(map, new Coor(0, 499 - min.X), true));
Console.WriteLine(SimulateSand(extendedMap, new Coor(0, 499 - min.X + extensionLeft), false));

static int SimulateSand(bool?[,] map, Coor start, bool fallOff)
{
    var down = new Coor(1, 0);
    var right = new Coor(1, 1);
    var left = new Coor(1, -1);
    var directions = new[] { down, left, right };
    var seeds = 0;

    while (true)
    {
        var current = start;
        bool moved = false;
        do
        {
            moved = false;
            foreach (var direction in directions)
            {
                var next = current + direction;
                if (!next.InBoundsOf(map))
                {
                    if (fallOff)
                        // First fall off
                        return seeds;
                    else
                        // Landed at the infinite bottom
                        break;
                }

                var nextValue = map.Get(next);
                if (!nextValue.HasValue)
                {
                    // Empty field
                    moved = true;
                    current = next;
                    break;
                }
            }
        } while (moved);

        // Map is full
        if (current == start)
        {
            return seeds + 1;
        }

        map.Set(current, true);
        seeds++;
    }
}
