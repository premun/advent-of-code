using AdventOfCode.Common;

using Coor = AdventOfCode.Common.Coor<int>;

var map = Resources.GetInputFileContent()
    .Split(Environment.NewLine)
    .Select(s => s.PadRight(150, ' '))
    .TakeWhile(s => !string.IsNullOrEmpty(s))
    .ParseAsArray(c => c);

var instructions = Resources.GetInputFileLines().Last();

var position = GetFirstEmptyCol(0);
var direction = 0;

for (int i = 0; i < instructions.Length; i++)
{
    //Console.Clear();
    //map.Print(c => c == position ? direction switch { 0 => '>', 1 => 'v', 2 => '<', 3 => '^' } : null);
    
    var instruction = instructions[i];
    if (instruction == 'L')
    {
        // Console.WriteLine("L");
        direction = (direction + 4 - 1) % 4;
        // Console.ReadKey();
        continue;
    }

    if (instruction == 'R')
    {
        // Console.WriteLine("R");
        direction = (direction + 1) % 4;
        // Console.ReadKey();
        continue;
    }

    var number = new string(instructions.Skip(i).TakeWhile(char.IsDigit).ToArray());
    // Console.WriteLine(number);
    var moves = int.Parse(number);
    i += number.Length - 1;

    for (int j = 0; j < moves; j++)
    {
        var newPosition = position + Coor.Directions[direction];
        var inBounds = newPosition.InBoundsOf(map);

        if (!inBounds || map.Get(newPosition) == ' ')
        {
            newPosition = direction switch
            {
                0 => GetFirstEmptyCol(position.Row, inBounds ? newPosition.Col : 0),
                1 => GetFirstEmptyRow(position.Col, inBounds ? newPosition.Row : 0),
                2 => GetLastEmptyCol(position.Row, inBounds ? newPosition.Col : 0),
                3 => GetLastEmptyRow(position.Col, inBounds ? newPosition.Row : 0),
                _ => throw new Exception()
            };
        }

        if (map.Get(newPosition) == '#')
        {
            break;
        }

        position = newPosition;
    }
    //Console.ReadKey();
}

Console.WriteLine($"Part 1: {1000 * (position.Row + 1) + 4 * (position.Col + 1) + direction}");

Coor GetFirstEmptyCol(int row, int start = 0) => new(row, Enumerable.Range(start, map!.GetLength(1) - start).Concat(Enumerable.Range(0, start)).First(col => map[row, col] != ' '));
Coor GetFirstEmptyRow(int col, int start = 0) => new(Enumerable.Range(start, map!.GetLength(0) - start).Concat(Enumerable.Range(0, start)).First(row => map[row, col] != ' '), col);
Coor GetLastEmptyCol(int row, int start = 0) => new(row, Enumerable.Range(start, map!.GetLength(1) - start).Concat(Enumerable.Range(0, start)).Last(col => map[row, col] != ' '));
Coor GetLastEmptyRow(int col, int start = 0) => new(Enumerable.Range(start, map!.GetLength(0) - start).Concat(Enumerable.Range(0, start)).Last(row => map[row, col] != ' '), col);
