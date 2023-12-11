using Coor = AdventOfCode.Common.Coor<int>;

namespace AdventOfCode._2022_17;

class Chamber
{
    private readonly List<bool[]> _world;
    private readonly bool[] _jetPattern;
    private int _currentInstruction;
    private int _nextRock = -1;

    public Chamber(int height, int width, bool[] jetPattern)
    {
        _world = [];
        _jetPattern = jetPattern;
        _currentInstruction = jetPattern.Length - 1;

        for (var i = 0; i < height; i++)
        {
            _world.Add(new bool[width]);
        }
    }

    public int Simulate(int rocks, bool display = false)
    {
        while (true)
        {
            var rock = GetNextRock();

            // Get top rock
            int top = _world.Count;
            for (int j = 0; j < top; j++)
            {
                if (_world[j].Any(c => c))
                {
                    top = j;
                    break;
                }
            }

            if (rocks == 0)
            {
                return _world.Count - top;
            }

            // Increase chamber depth so that new rock has 3 spaces below
            while (top < rock.Height + 3)
            {
                _world.Insert(0, new bool[_world[0].Length]);
                top++;
            }

            while (top > rock.Height + 3)
            {
                _world.RemoveAt(0);
                top--;
            }

            do
            {
                if (display) Display(rock);

                if (GetNextInstruction())
                {
                    MoveRight(rock);
                }
                else
                {
                    MoveLeft(rock);
                }

                if (display) Display(rock);
            } while (MoveDown(rock));

            foreach (var c in rock.Positions)
            {
                _world[c.Y][c.X] = true;
            }

            rocks--;
        }
    }

    private void MoveRight(Rock rock)
    {
        if (rock.Positions.Any(p => p.X == _world[0].Length - 1 || _world[p.Y][p.X + 1]))
        {
            return;
        }

        rock.Positions = rock.Positions.Select(p => new Coor(p.Y, p.X + 1)).ToList();
    }

    private void MoveLeft(Rock rock)
    {
        if (rock.Positions.Any(p => p.X == 0 || _world[p.Y][p.X - 1]))
        {
            return;
        }

        rock.Positions = rock.Positions.Select(p => new Coor(p.Y, p.X - 1)).ToList();
    }

    private bool MoveDown(Rock rock)
    {
        if (rock.Positions.Any(p => p.Y == _world.Count - 1 || _world[p.Y + 1][p.X]))
        {
            return false;
        }

        rock.Positions = rock.Positions.Select(p => new Coor(p.Y + 1, p.X)).ToList();
        return true;
    }

    private bool GetNextInstruction()
    {
        _currentInstruction += 1;
        _currentInstruction %= _jetPattern.Length;
        return _jetPattern[_currentInstruction];
    }

    private Rock GetNextRock()
    {
        _nextRock += 1;
        _nextRock %= 5;
        return _nextRock switch
        {
            0 => new RowRock(),
            1 => new CrossRock(),
            2 => new LRock(),
            3 => new ColumnRock(),
            4 => new BoxRock(),
            _ => throw new Exception("Invalid rock")
        };
    }

    private void Display(Rock rock)
    {
        char DisplayChar(int y, int x, bool isMapRock)
        {
            if (isMapRock) return '#';
            if (rock.Positions.Contains(new Coor(y, x))) return '@';
            return '.';
        }

        Console.Clear();
        for (var y = 0; y < _world.Count; y++)
        {
            var row = _world[y];
            Console.WriteLine(new string(row.Select((b, x) => DisplayChar(y, x, b)).ToArray()));
        }

        Console.WriteLine();
        Console.WriteLine(new string(_jetPattern.Select(p => p ? '>' : '<').ToArray()));
        Console.WriteLine(new string(' ', _currentInstruction) + '^');

        Console.ReadKey();
    }
}
