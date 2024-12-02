using AdventOfCode.Common;

var commands = Resources.GetInputFileLines();
var system = new CommunicationSystem(40, [20, 60, 100, 140, 180, 220]);
system.RunCommands(commands);

Console.WriteLine($"Part 1: {system.Signal}");

class CommunicationSystem(int displayWidth, IEnumerable<int> importantCycles)
{
    private readonly int _displayWidth = displayWidth;
    private readonly Queue<int> _importantCycles = new(importantCycles);
    private int _register;
    private int _cycle;

    public int Signal { get; private set; }

    public void RunCommands(IEnumerable<string> commands)
    {
        _cycle = 0;
        _register = 1;
        Signal = 0;

        var nextCycle = _importantCycles.Dequeue();

        foreach (var command in commands)
        {
            Tick();
            
            var current = 0;
            if (command is ['a', 'd', 'd', 'x', ' ', .. string value])
            {
                current = int.Parse(value);
                Tick();
            }

            if (_cycle >= nextCycle)
            {
                Signal += nextCycle * _register;
                _importantCycles.TryDequeue(out nextCycle);
            }

            _register += current;
        }
    }

    private void Tick()
    {
        DrawPixel();
        _cycle++;
    }

    private void DrawPixel()
    {
        var position = _cycle % _displayWidth;
        Console.Write(Math.Abs(position - _register) < 2 ? '#' : '.');

        if (position == _displayWidth - 1)
        {
            Console.WriteLine();
        }
    }
}
