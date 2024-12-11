using System.Collections;
using AdventOfCode.Common;

var values = Resources.GetInputFileLines()
    .First()
    .SplitToNumbers(" ");

var head = Initialize(values);

for (int i = 0; i < 75; ++i)
{
    Console.WriteLine(i);

    var stone = head;
    while (stone != null)
    {
        stone = stone.Blink();
    }

    //Console.WriteLine(string.Join(", ", head));
}

Console.WriteLine($"Part 1: {head.Count()}");
Console.WriteLine($"Part 2: {""}");

static Stone Initialize(int[] values)
{
    var head = new Stone((ulong)values.First());
    var previous = head;
    for (int i = 1; i < values.Length; i++)
    {
        var newStone = new Stone((ulong)values[i], previous, null);
        if (previous != null)
            previous.Next = newStone;

        previous = newStone;
    }

    return head;
}

file class Stone(ulong number, Stone? previous = null, Stone? next = null) : IEnumerable<ulong>
{
    public ulong Number { get; set; } = number;
    public Stone? Previous { get; set; } = previous;
    public Stone? Next { get; set; } = next;

    public Stone? Blink()
    {
        if (Number == 0)
        {
            Number = 1;
            return Next;
        }

        var digits = (int)(Math.Log10(Number) + 1);
        if (digits % 2 == 0)
        {
            var div = (ulong)Math.Pow(10, digits / 2);
            var left = Number / div;
            var right = Number - left * div;

            var newStone = new Stone(right, this, Next);

            Number = left;
            Next = newStone;

            return newStone.Next;
        }

        Number *= 2024;
        return Next;
    }

    public IEnumerator<ulong> GetEnumerator() => new StoneEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class StoneEnumerator(Stone head) : IEnumerator<ulong>
    {
        private readonly Stone _head = head;
        private Stone? _current = null;

        public ulong Current => _current!.Number;

        object IEnumerator.Current => Current;

        public void Dispose() {}
        public bool MoveNext()
        {
            if (_current == null)
            {
                _current = _head;
                return true;
            }

            if (_current.Next == null)
            {
                return false;
            }

            _current = _current.Next;
            return true;
        }

        public void Reset() => _current = _head;
    }
}
